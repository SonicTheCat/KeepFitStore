namespace KeepFitStore.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Services.Contracts;
    using System.Linq;
    using KeepFitStore.Models.ViewModels.Basket;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;

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

        public async Task EdintBasketItemAsync(int basketId, int productId, int quantity)
        {
            var basketItem = this.context
                .BasketItems
                .SingleOrDefault(x => x.BasketId == basketId && x.ProductId == productId);

            if (basketItem == null)
            {
                //TODO throw Service exception
                return; 
            }

            basketItem.Quantity = quantity;
            await this.context.SaveChangesAsync(); 
        }

        public async Task<IEnumerable<IndexBasketViewModel>> GetBasketContentAsync(ClaimsPrincipal principal)
        {
            var user = await this.userManager.GetUserAsync(principal);
            var basketId = user.BasketId;

            var productsInBasket = await this.context
                .BasketItems
                .Include(x => x.Basket)
                .Include(x => x.Product)
                .Where(x => x.Basket.Id == basketId)
                .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<IndexBasketViewModel>>(productsInBasket);
            return viewModel;
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