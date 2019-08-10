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
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.CustomExceptions.Messsages;
    using KeepFitStore.Services.Common;

    public class OrdersService : IOrdersService
    {
        private readonly KeepFitDbContext context;  
        private readonly IBasketService basketService;
        private readonly IMapper mapper;

        public OrdersService(KeepFitDbContext context, IBasketService basketService, IMapper mapper)
        {
            this.context = context;
            this.basketService = basketService;
            this.mapper = mapper;
        }

        public async Task<CreateOrderInputModel> AddBasketContentToOrderByUserAsync(string username)
        {
            var user = await this.GetUserWithAllPropertiesAsync(username);

            var order = new CreateOrderInputModel
            {
                User = this.mapper.Map<CreateOrderUserInputModel>(user),
                Products = this.mapper.Map<ICollection<CreateOrderProductInputModel>>(user.Basket.BasketItems)
            };

            if (order.Products.Count == ServicesConstants.InvalidProductsCountInOrder)
            {
                throw new ServiceException(string.Format(ExceptionMessages.InvalidBasketContent, user.Id));
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

        public async Task<int> StartCompletingUserOderAsync(string username, CreateOrderInputModel model)
        {
            var user = await this.GetUserWithAllPropertiesAsync(username);
            var order = this.mapper.Map<Order>(model);

            order.KeepFitUser = user;
            order.ReceiverFullName = user.FullName;
            order.ReceiverPhoneNumber = user.PhoneNumber; 
            order.DeliveryAddress = user.Address;
            order.Products = this.mapper.Map<ICollection<ProductOrder>>(user.Basket.BasketItems);

            if (order.Products.Count == ServicesConstants.InvalidProductsCountInOrder)
            {
                throw new ServiceException(string.Format(ExceptionMessages.InvalidBasketContent, user.Id));
            }

            var basketPriceWithoutDelivery = await this.basketService.GetBasketTotalPriceAsync(username);
            this.CalculateDeliveryPrice(order, basketPriceWithoutDelivery);
            order.TotalPrice = basketPriceWithoutDelivery + order.DeliveryPrice;
            this.CalculateDeliveryDate(order);

            this.context.Orders.Add(order);
            await this.context.SaveChangesAsync();

            return order.Id;
        }

        public async Task CompleteOrderAsync(string username, int orderId)
        {
            var order = await this.context
                 .Orders
                 .SingleOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
            {
                throw new OrderNotFoundException(string.Format(ExceptionMessages.OrderNotFound, orderId));
            }

            if (order.IsCompleted)
            {
                return;
            }

            var user = await this.GetUserWithAllPropertiesAsync(username);
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
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
            {
                throw new OrderNotFoundException(string.Format(ExceptionMessages.OrderNotFound, orderId));
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
                .AsNoTracking()
                .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(orders);
            return viewModel;
        }

        public async Task<IEnumerable<TViewModel>> GetAllOrdersForUserAsync<TViewModel>(string username)
        {
            ThrowIfUserIsNotExist(username);

            var orders = await this.context
                .Orders
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .Where(x => x.KeepFitUser.UserName == username)
                .AsNoTracking()
                .ToListAsync();

            var ordersViewModel = this.mapper.Map<IEnumerable<TViewModel>>(orders);
            return ordersViewModel;
        }

        public async Task<IEnumerable<TViewModel>> GetAllOrdersForUserSortedAsync<TViewModel>(
            string username,
            string sortBy)
        {
            ThrowIfUserIsNotExist(username);

            sortBy = sortBy.Replace(ServicesConstants.DescendingOldValue, ServicesConstants.DescendingNewValue);

            var orders = await this.context
               .Orders
               .Include(x => x.Products)
               .ThenInclude(x => x.Product)
               .Where(x => x.KeepFitUser.UserName == username)
               .OrderBy(sortBy)
               .AsNoTracking()
               .ToListAsync();

            var ordersViewModel = this.mapper.Map<IEnumerable<TViewModel>>(orders);
            return ordersViewModel;
        }

        public async Task<IEnumerable<TViewModel>> AppendFiltersAndSortOrdersAsync<TViewModel>(
            string[] filters,
            string sortBy)
        {
            sortBy = sortBy.Replace(ServicesConstants.DescendingOldValue, ServicesConstants.DescendingNewValue);

            var orders = await this.context
             .Orders
             .Include(x => x.Products)
             .ThenInclude(x => x.Product)
             .Include(x => x.KeepFitUser)
             .Where(x => x.IsCompleted)
             .Where(x => filters.Length == 0 ? true : filters.Contains(x.Status.ToString()))
             .OrderBy(sortBy)
             .AsNoTracking()
             .ToListAsync();

            var ordersViewModel = this.mapper.Map<IEnumerable<TViewModel>>(orders);
            return ordersViewModel;
        }

        public async Task<TViewModel> GetOrderDetailsForUserAsync<TViewModel>(string username, int orderId)
        {
            ThrowIfUserIsNotExist(username);

            var order = await this.context
                .Orders
                .Include(x => x.KeepFitUser)
                .Include(x => x.DeliveryAddress)
                .ThenInclude(x => x.City)
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
            {
                throw new OrderNotFoundException(string.Format(ExceptionMessages.OrderNotFound, orderId));
            }

            if (username != order.KeepFitUser.UserName)
            {
                throw new UserNotAuthorizedException(string.Format(ExceptionMessages.NotAuthorized, username));
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
               .AsNoTracking()
               .SingleOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
            {
                throw new OrderNotFoundException(string.Format(ExceptionMessages.OrderNotFound, orderId));
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
                throw new OrderNotFoundException(string.Format(ExceptionMessages.OrderNotFound, orderId));
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
            int hours = default;
            if (deliveryType == DeliveryType.Express)
                hours = ServicesConstants.ExpressDeliveryHours;
            else if (deliveryType == DeliveryType.NextDay)
                hours = ServicesConstants.NextDayDeliveryHours;
            else if (deliveryType == DeliveryType.Standart)
                hours = ServicesConstants.StandartDeliveryHours;

            order.DeliveryDate = order.OrderDate.Value.AddHours(hours);
        }

        private void CalculateDeliveryPrice(Order order, decimal basketPriceWithoutDelivery)
        {
            var deliveryType = order.DeliveryType;
            decimal price = default;

            if (this.IsFreeDelivery(deliveryType, basketPriceWithoutDelivery))
            {
                order.DeliveryPrice = price;
                return;
            }

            if (deliveryType == DeliveryType.Express)
                price = ServicesConstants.ExpressDeliveryPrice;
            else if (deliveryType == DeliveryType.NextDay)
                price = ServicesConstants.NextDayDeliveryPrice;
            else if (deliveryType == DeliveryType.Standart)
                price = ServicesConstants.StandartDeliveryPrice;

            order.DeliveryPrice = price;
        }

        private bool IsFreeDelivery(DeliveryType deliveryType, decimal basketPriceWithoutDelivery)
        {
            if ((basketPriceWithoutDelivery >= ServicesConstants.MinimumBasketValueForNextDayDelivery &&
              deliveryType != DeliveryType.Express) ||
              (basketPriceWithoutDelivery >= ServicesConstants.MinimumBasketValueForStandartDeliery &&
              deliveryType == DeliveryType.Standart))
            {
                return true;
            }

            return false;
        }

        private async Task<KeepFitUser> GetUserWithAllPropertiesAsync(string username)
        {
            var user = await this.context
                .Users
                .Include(x => x.Basket)
                .ThenInclude(x => x.BasketItems)
                .ThenInclude(x => x.Product)
                .Include(x => x.Address)
                .ThenInclude(x => x.City)
                .SingleOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                throw new UserNotFoundException(string.Format(ExceptionMessages.UserDoesNotExist, username));
            }

            return user;
        }

        private void ThrowIfUserIsNotExist(string username)
        {
            if (!this.context.Users.Any(x => x.UserName == username))
            {
                throw new UserNotFoundException(string.Format(ExceptionMessages.UserDoesNotExist, username));
            }
        }
    }
}