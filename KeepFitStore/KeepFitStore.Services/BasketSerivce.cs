﻿namespace KeepFitStore.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Services.Contracts;
    using System.Linq;
 
    public class BasketSerivce : IBasketService
    {
        private const int QuantityDefaultValue = 1;

        private readonly KeepFitDbContext context;
        private readonly IProductsService productsService;
        private readonly UserManager<KeepFitUser> userManager;
        private readonly IMapper mapper;

        public BasketSerivce(KeepFitDbContext context, IProductsService productsService, UserManager<KeepFitUser> userManager, IMapper mapper)
        {
            this.context = context;
            this.productsService = productsService;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task AddProductToBasketAsync(int productId, ClaimsPrincipal principal, int? quntity = null)
        {
            var product = await this.productsService.GetProductByIdAsync(productId);
            var user = await this.userManager.GetUserAsync(principal);

            if (product == null || user == null)
            {
                //TODO: throw ServiceError 
                return;
            }

            var basketItem = GetBasketItemOrDefault(product.Id, user.BasketId);
            if (basketItem != null)
            {
                return;
            }

            basketItem = new BasketItem()
            {
                ProductId = product.Id,
                Quantity = quntity.HasValue ? quntity.Value : QuantityDefaultValue,
                BasketId = user.BasketId
            };

            this.context.BasketItems.Add(basketItem);
            await this.context.SaveChangesAsync(); 
        }

        private BasketItem GetBasketItemOrDefault(int productId, int basketId)
        {
            var basektItem = this.context
                .BasketItems
                .FirstOrDefault(x => x.ProductId == productId && x.BasketId == basketId);

            return basektItem;
        }
    }
}