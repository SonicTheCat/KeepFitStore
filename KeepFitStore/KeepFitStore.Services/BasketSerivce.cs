﻿namespace KeepFitStore.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.ViewModels.Basket;

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
                basketItem.Quantity++;
            }
            else
            {
                basketItem = new BasketItem()
                {
                    ProductId = product.Id,
                    Quantity = quntity.HasValue ? quntity.Value : QuantityDefaultValue,
                    BasketId = user.BasketId
                };

                this.context.BasketItems.Add(basketItem);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<EditBasketItemViewModel> EditBasketItemAsync(int basketId, int productId, int quantity)
        {
            var basketItem = this.context
                .BasketItems
                .Include(x => x.Product)
                .SingleOrDefault(x => x.BasketId == basketId && x.ProductId == productId);

            if (basketItem == null || quantity <= 0)
            {
                //TODO throw Service exception
            }

            basketItem.Quantity = quantity;
            await this.context.SaveChangesAsync();

            var viewModel = this.mapper.Map<EditBasketItemViewModel>(basketItem);
            return viewModel;
        }

        public async Task<IEnumerable<BasketViewModel>> GetBasketContentAsync(ClaimsPrincipal principal)
        {
            var basketItems = await this.GetItemsAsync(principal);

            var viewModel = this.mapper.Map<IEnumerable<BasketViewModel>>(basketItems);
            return viewModel;
        }

        public async Task<decimal> GetBasketTotalPriceAsync(ClaimsPrincipal principal)
        {
            var basketItems = await this.GetItemsAsync(principal);
            var total = basketItems.Sum(x => x.Quantity * x.Product.Price);
            return total;
        }

        public async Task<bool> DeleteBasketItemAsync(int basketId, int productId)
        {
            var basketItem = this.GetBasketItemOrDefault(productId, basketId);

            if (basketItem == null)
            {
                //TODO: throw service exception
                return false;
            }

            this.context.BasketItems.Remove(basketItem);
            var deletedCount = await this.context.SaveChangesAsync();

            if (deletedCount != 1)
            {
                return false;
            }

            return true;
        }

        public async Task ClearBasketAsync(int basketId)
        {
            var basket = await this.context
                .Baskets
                .Include(x => x.BasketItems)
                .SingleOrDefaultAsync(x => x.Id == basketId);

            this.context.BasketItems.RemoveRange(basket.BasketItems);
            await this.context.SaveChangesAsync();
        }

        private async Task<List<BasketItem>> GetItemsAsync(ClaimsPrincipal principal)
        {
            var user = await this.userManager.GetUserAsync(principal);
            if (user == null)
            {
                //TODO: throw service error
            }

            var basketId = user.BasketId;

            var productsInBasket = await this.context
                .BasketItems
                .Include(x => x.Basket)
                .Include(x => x.Product)
                .Where(x => x.Basket.Id == basketId)
                .ToListAsync();

            return productsInBasket; 
        }

        private BasketItem GetBasketItemOrDefault(int productId, int basketId)
        {
            var basektItem = this.context
                .BasketItems
                .Include(x => x.Basket)
                .Include(x => x.Product)
                .FirstOrDefault(x => x.ProductId == productId && x.BasketId == basketId);

            return basektItem;
        }
    }
}