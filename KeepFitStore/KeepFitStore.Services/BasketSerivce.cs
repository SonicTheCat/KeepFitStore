namespace KeepFitStore.Services
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
    using KeepFitStore.Models.ViewModels.Products;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.CustomExceptions.Messsages;
    using KeepFitStore.Services.Common;

    public class BasketSerivce : IBasketService
    {
        private readonly KeepFitDbContext context;
        private readonly IProductsService productsService;
        private readonly IMapper mapper;

        public BasketSerivce(KeepFitDbContext context, IProductsService productsService, IMapper mapper)
        {
            this.context = context;
            this.productsService = productsService;
            this.mapper = mapper;
        }

        public async Task AddProductToBasketAsync(int productId, string username, int? quntity = null)
        {
            var product = await this.productsService.GetProductByIdAsync<ProductViewModel>(productId);
            var user = await this.context.Users.SingleOrDefaultAsync(x => x.UserName == username); 
           
            if (user == null)
            {
                throw new UserNotFoundException(string.Format(ExceptionMessages.UserDoesNotExist, username));
            }

            if (product == null)
            {
                throw new ProductNotFoundException(string.Format(ExceptionMessages.ProductNotFound, productId));
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
                    Quantity = quntity.HasValue ? quntity.Value : ServicesConstants.QuantityDefaultValue,
                    BasketId = user.BasketId
                };

                this.context.BasketItems.Add(basketItem);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<TViewModel> EditBasketItemAsync<TViewModel>(int basketId, int productId, int quantity)
        {
            var basketItem = this.context
                .BasketItems
                .Include(x => x.Product)
                .SingleOrDefault(x => x.BasketId == basketId && x.ProductId == productId);

            if (basketItem == null)
            {
                throw new BasketItemNotFoundException(string.Format(ExceptionMessages.BasketItemNotFound, basketId, productId));
            }

            if (quantity <= ServicesConstants.InvalidQuantityNumber)
            {
                throw new InvalidQuantityProvidedException(string.Format(ExceptionMessages.NegativeQuantity));
            }

            basketItem.Quantity = quantity;
            await this.context.SaveChangesAsync();

            var viewModel = this.mapper.Map<TViewModel>(basketItem);
            return viewModel;
        }

        public async Task<IEnumerable<TViewModel>> GetBasketContentAsync<TViewModel>(string username)
        {
            var basketItems = await this.GetItemsAsync(username);

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(basketItems);
            return viewModel;
        }

        public async Task<decimal> GetBasketTotalPriceAsync(string username)
        {
            var basketItems = await this.GetItemsAsync(username);
            var total = basketItems.Sum(x => x.Quantity * x.Product.Price);
            return total;
        }

        public async Task<bool> DeleteBasketItemAsync(int basketId, int productId)
        {
            var basketItem = this.GetBasketItemOrDefault(productId, basketId);

            if (basketItem == null)
            {
                throw new BasketItemNotFoundException(string.Format(ExceptionMessages.BasketItemNotFound, basketId, productId));
            }

            this.context.BasketItems.Remove(basketItem);
            var deletedCount = await this.context.SaveChangesAsync();

            if (deletedCount != ServicesConstants.OneRow)
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

            if (basket == null)
            {
               throw new ServiceException(ServicesConstants.BasketNotFound); 
            }

            this.context.BasketItems.RemoveRange(basket.BasketItems);
            await this.context.SaveChangesAsync();
        }

        private async Task<List<BasketItem>> GetItemsAsync(string username)
        {
            var user = await this.context.Users.SingleOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                throw new UserNotFoundException(string.Format(ExceptionMessages.UserDoesNotExist, username));
            }

            var basketId = user.BasketId;

            var productsInBasket = await this.context
                .BasketItems
                .Include(x => x.Basket)
                .Include(x => x.Product)
                .Where(x => x.Basket.Id == basketId)
                .AsNoTracking()
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