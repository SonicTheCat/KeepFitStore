namespace KeepFitStore.Services
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using KeepFitStore.Domain;
    using KeepFitStore.Models.InputModels.Orders;
    using KeepFitStore.Models.ViewModels.Basket;
    using KeepFitStore.Services.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class OrdersService : IOrdersService
    {
        private readonly UserManager<KeepFitUser> userManager;

        public OrdersService(UserManager<KeepFitUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<CreateOrderInputModel> AddBasketContentToOrderByUserAsync(ClaimsPrincipal principal)
        {
            var user = await this.userManager.GetUserAsync(principal);

            var userWithProperties = await this.userManager
                .Users
                .Include(x => x.Basket)
                .ThenInclude(x => x.BasketItems)
                .ThenInclude(x => x.Product)
                .Include(x => x.Address)
                .ThenInclude(x => x.City)
                .SingleOrDefaultAsync(x => x.Id == user.Id); 

            var order = new CreateOrderInputModel();

            foreach (var basketItem in userWithProperties.Basket.BasketItems)
            {
                order.Products.Add(new CreateOrderProductInputModel
                {
                    Id = basketItem.ProductId,
                    Quantity = basketItem.Quantity,
                    Name = basketItem.Product.Name,
                    Price = basketItem.Product.Price,
                    ProductType = basketItem.Product.ProductType
                });
            }

            order.User = new CreateOrderUserInputModel
            {
                Id = userWithProperties.Id,
                FullName = userWithProperties.FullName,
                Email = userWithProperties.Email,
                PhoneNumber = userWithProperties.PhoneNumber
            };

            if (userWithProperties.Address != null)
            {
                order.DeliveryAddress = new CreateOrderAddressInputModel
                {
                    Id = userWithProperties.Address.Id,
                    StreetName = userWithProperties.Address.StreetName,
                    StreetNumber = userWithProperties.Address.StreetNumber,
                    BuildingNumebr = userWithProperties.Address.BuildingNumebr,
                    RegionName = userWithProperties.Address.RegionName,
                    City = new CreateOrderCityInputModel
                    {
                        Id = userWithProperties.Address.City.Id,
                        Name = userWithProperties.Address.City.Name,
                        PostCode = userWithProperties.Address.City.PostCode
                    }
                };
            }

            return order;
        }
    }
}