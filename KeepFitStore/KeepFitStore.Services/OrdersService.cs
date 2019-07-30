namespace KeepFitStore.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System;
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
        private const string DescendingOldValue = "Desc";
        private const string DescendingNewValue = " descending";

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
            var user = await this.GetUserWithAllPropertiesAsync(principal);

            var order = new CreateOrderInputModel
            {
                User = this.mapper.Map<CreateOrderUserInputModel>(user),
                Products = this.mapper.Map<ICollection<CreateOrderProductInputModel>>(user.Basket.BasketItems)
            };

            if (order.Products.Count == 0)
            {
                //TODO throw service error, dont return null
                return null;
            }

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

        public async Task<int> StartCompletingUserOderAsync(ClaimsPrincipal principal, CreateOrderInputModel model)
        {
            var user = await this.GetUserWithAllPropertiesAsync(principal);
            var order = this.mapper.Map<Order>(model);

            order.KeepFitUser = user;
            order.DeliveryAddress = user.Address;
            order.Products = this.mapper.Map<ICollection<ProductOrder>>(user.Basket.BasketItems);

            if (order.Products.Count == 0)
            {
                //TODO: throw service error
            }

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

            return order.Id;
        }

        public async Task CompleteOrderAsync(ClaimsPrincipal principal, int orderId)
        {
            var order = await this.context
                 .Orders
                 .SingleOrDefaultAsync(x => x.Id == orderId);

            if (order == null || order.IsCompleted)
            {
                //TODO: throw service error
            }

            var user = await this.GetUserWithAllPropertiesAsync(principal);
            await this.basketService.ClearBasketAsync(user.BasketId);
            order.IsCompleted = true;
            order.Status = OrderStatus.Assembling; 

            await this.context.SaveChangesAsync(); 
        }

        public async Task<TViewModel> GetOrderByIdAsync<TViewModel>(int orderId)
        {
            var order = await this.context
                .Orders
                .Include(x => x.KeepFitUser)
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .Include(x => x.DeliveryAddress)
                .SingleOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
            {
                //TODO: throw service error
            }

            var viewModel = this.mapper.Map<TViewModel>(order);

            return viewModel; 
        }

        public async Task<IEnumerable<TViewModel>> GetAllOrdersAsync<TViewModel>()
        {
            var orders = await this.context
                .Orders
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .Include(x => x.KeepFitUser)
                .Where(x => x.IsCompleted)
                .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(orders);
            return viewModel;
        }

        public async Task<IEnumerable<TViewModel>> GetAllOrdersForUserAsync<TViewModel>(ClaimsPrincipal principal)
        {
            var userId = this.userManager.GetUserId(principal);

            if (userId == null)
            {
                //TODO: throw service error
            }

            var orders = await this.context
                .Orders
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .Where(x => x.KeepFitUserId == userId)
                .ToListAsync();

            var ordersViewModel = this.mapper.Map<IEnumerable<TViewModel>>(orders);
            return ordersViewModel;
        }

        public async Task<IEnumerable<TViewModel>> GetAllOrdersForUserSortedAsync<TViewModel>(
            ClaimsPrincipal principal,
            string sortBy)
        {
            var userId = this.userManager.GetUserId(principal);

            if (userId == null || sortBy == null)
            {
                //TODO: throw service error
            }

            sortBy = sortBy.Replace(DescendingOldValue, DescendingNewValue);

            var orders = await this.context
               .Orders
               .Include(x => x.Products)
               .ThenInclude(x => x.Product)
               .Where(x => x.KeepFitUserId == userId)
               .OrderBy(sortBy)
               .ToListAsync();

            var ordersViewModel = this.mapper.Map<IEnumerable<TViewModel>>(orders);
            return ordersViewModel;
        }

        public async Task<IEnumerable<TViewModel>> AppendFiltersAndSortOrdersAsync<TViewModel>(
            string[] filters, 
            string sortBy)
        {
            if (sortBy == null)
            {
                //TODO: throw service error
            }

            sortBy = sortBy.Replace(DescendingOldValue, DescendingNewValue);

            var orders = await this.context
             .Orders
             .Include(x => x.Products)
             .ThenInclude(x => x.Product)
             .Include(x => x.KeepFitUser)
             .Where(x => filters.Length == 0 ? true : filters.Contains(x.Status.ToString()))
             .OrderBy(sortBy)
             .ToListAsync();

            var ordersViewModel = this.mapper.Map<IEnumerable<TViewModel>>(orders);
            return ordersViewModel;
        }

        public async Task<TViewModel> GetOrderDetailsForUserAsync<TViewModel>(ClaimsPrincipal principal, int orderId)
        {
            var userId = this.userManager.GetUserId(principal);

            var order = await this.context
                .Orders
                .Include(x => x.KeepFitUser)
                .Include(x => x.DeliveryAddress)
                .ThenInclude(x => x.City)
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .SingleOrDefaultAsync(x => x.Id == orderId);

            if (userId == null || order == null)
            {
                //TODO: throw service error
            }

            if (userId != order.KeepFitUserId)
            {
                //TODO: throw service error 
            }

            var orderViewModel = this.mapper.Map<TViewModel>(order);
            return orderViewModel;
        }

        public async Task<TViewModel> GetOrderDetailsAsync<TViewModel>(int orderId)
        {
            var order = await this.context
               .Orders
               .Include(x => x.KeepFitUser)
               .Include(x => x.DeliveryAddress)
               .ThenInclude(x => x.City)
               .Include(x => x.Products)
               .ThenInclude(x => x.Product)
               .SingleOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
            {
                //TODO: throw service error
            }

            var orderViewModel = this.mapper.Map<TViewModel>(order);

            return orderViewModel;
        }

        public async Task ChangeOrderCurrentStatusAsync(int orderId, string currentStatus)
        {
            var order = await this.context
                .Orders
                .SingleOrDefaultAsync(x => x.Id == orderId);

            var isValidOrderStatus = Enum.TryParse(typeof(OrderStatus), currentStatus, out _);

            if (order == null || !isValidOrderStatus)
            {
                //TODO: throw service error
            }

            if (OrderStatus.Assembling.ToString() == currentStatus)
            {
                order.Status = OrderStatus.Shipment;
            }
            else if (OrderStatus.Shipment.ToString() == currentStatus)
            {
                order.Status = OrderStatus.Delivered;
            }

            await this.context.SaveChangesAsync();
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

        private async Task<KeepFitUser> GetUserWithAllPropertiesAsync(ClaimsPrincipal principal)
        {
            var userId = this.userManager.GetUserId(principal);

            if (userId == null)
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
                .SingleOrDefaultAsync(x => x.Id == userId);

            return userWithProperties;
        }
    }
}