namespace KeepFitStore.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Domain;
    using KeepFitStore.Models.InputModels.Orders;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Data;

    public class OrdersService : IOrdersService
    {
        private const int MinimumBasketValueForNextDayDelivery = 60;
        private const int MinimumBasketValueForStandartDeliery = 20;
        private const int ExpressDeliveryPrice = 15;
        private const int NextDayDeliveryPrice = 10;
        private const int StandartDeliveryPrice = 5;
        private const int ExpressDeliveryHours = 5;
        private const int NextDayDeliveryHours = 24;
        private const int StandartDeliveryHours = 92;

        private readonly KeepFitDbContext context;
        private readonly UserManager<KeepFitUser> userManager;
        private readonly IBasketService basketService;
        private readonly IMapper mapper;

        public OrdersService(KeepFitDbContext context, UserManager<KeepFitUser> userManager, IBasketService basketService, IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.basketService = basketService;
            this.mapper = mapper;
        }

        public async Task<CreateOrderInputModel> AddBasketContentToOrderByUserAsync(ClaimsPrincipal principal)
        {
            var user = await this.GetUserWithAllProperties(principal);

            var order = new CreateOrderInputModel
            {
                User = this.mapper.Map<CreateOrderUserInputModel>(user),
                Products = this.mapper.Map<ICollection<CreateOrderProductInputModel>>(user.Basket.BasketItems)
            };

            if (user.Address != null)
            {
                order.DeliveryAddress = new CreateOrderAddressInputModel
                {
                    Id = user.Address.Id,
                    StreetName = user.Address.StreetName,
                    StreetNumber = user.Address.StreetNumber,
                    City = new CreateOrderCityInputModel
                    {
                        Id = user.Address.City.Id,
                        Name = user.Address.City.Name,
                        PostCode = user.Address.City.PostCode
                    }
                };
            }

            return order;
        }

        public async Task StartCompletingUserOder(ClaimsPrincipal principal, CreateOrderInputModel model)
        {
            var user = await this.GetUserWithAllProperties(principal);
            var order = this.mapper.Map<Order>(model);

            order.KeepFitUser = user;
            order.DeliveryAddress = user.Address;
            order.Products = this.mapper.Map<ICollection<ProductOrder>>(user.Basket.BasketItems);

            var basketPriceWithoutDelivery = await this.basketService.GetBasketTotalPriceAsync(principal);
            this.CalculateDeliveryPrice(order, basketPriceWithoutDelivery);
            order.TotalPrice = basketPriceWithoutDelivery + order.DeliveryPrice;
            this.CalculateDeliveryDate(order);

            this.context.Orders.Add(order);
            var isAdded = await this.context.SaveChangesAsync();

            if (isAdded != 1)
            {
                //TODO: throw service error
            }

            await this.basketService.ClearBasketAsync(user.BasketId); 
        }

        private void CalculateDeliveryDate(Order order)
        {
            var deliveryType = order.DeliveryType;
            var hours = 0;
            if (deliveryType == DeliveryType.Express)
                hours = ExpressDeliveryHours;
            else if (deliveryType == DeliveryType.NextDay)
                hours = NextDayDeliveryHours;
            else if (deliveryType == DeliveryType.Standart)
                hours = StandartDeliveryHours;

            order.DeliveryDate = order.OrderDate.Value.AddHours(hours);
        }

        private void CalculateDeliveryPrice(Order order, decimal basketPriceWithoutDelivery)
        {
            var deliveryType = order.DeliveryType;
            var price = 0m;

            if (this.IsFreeDelivery(deliveryType, basketPriceWithoutDelivery))
            {
                order.DeliveryPrice = price;
                return;
            }

            if (deliveryType == DeliveryType.Express)
                price = ExpressDeliveryPrice;
            else if (deliveryType == DeliveryType.NextDay)
                price = NextDayDeliveryPrice;
            else if (deliveryType == DeliveryType.Standart)
                price = StandartDeliveryPrice;

            order.DeliveryPrice = price;
        }

        private bool IsFreeDelivery(DeliveryType deliveryType, decimal basketPriceWithoutDelivery)
        {
            if ((basketPriceWithoutDelivery >= MinimumBasketValueForNextDayDelivery &&
              deliveryType != DeliveryType.Express) ||
              (basketPriceWithoutDelivery >= MinimumBasketValueForStandartDeliery &&
              deliveryType == DeliveryType.Standart))
            {
                return true;
            }

            return false;
        }

        private async Task<KeepFitUser> GetUserWithAllProperties(ClaimsPrincipal principal)
        {
            var user = await this.userManager.GetUserAsync(principal);

            if (user == null)
            {
                //TODO: throw service error
            }

            var userWithProperties = await this.userManager
                .Users
                .Include(x => x.Basket)
                .ThenInclude(x => x.BasketItems)
                .ThenInclude(x => x.Product)
                .Include(x => x.Address)
                .ThenInclude(x => x.City)
                .SingleOrDefaultAsync(x => x.Id == user.Id);

            return userWithProperties;
        }
    }
}