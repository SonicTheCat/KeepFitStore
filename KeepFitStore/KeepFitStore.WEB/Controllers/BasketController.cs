namespace KeepFitStore.WEB.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using AutoMapper;

    using KeepFitStore.Helpers;
    using KeepFitStore.Models.ViewModels.Basket;
    using KeepFitStore.Models.ViewModels.Products;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Common;
    using KeepFitStore.Models.InputModels.Basket;
    public class BasketController : BaseController
    {
        private const int BasketItemInSessionDefaultQuantityValue = 0;

        private readonly IBasketService basketService;
        private readonly IProductsService productsService;
        private readonly IMapper mapper;

        public BasketController(IBasketService basketService, IProductsService productsService, IMapper mapper)
        {
            this.basketService = basketService;
            this.productsService = productsService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var basketContent = await this.basketService.GetBasketContentAsync<BasketViewModel>(this.User);
                return this.View(basketContent);
            }

            var basketSession = SessionHelper.GetObjectFromJson<List<BasketViewModel>>(this.HttpContext.Session, WebConstants.BasketKey);

            return this.View(basketSession);
        }

        public async Task<IActionResult> Add(int id)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                await this.basketService.AddProductToBasketAsync(id, this.User);
            }
            else
            {
                var basketSession = SessionHelper.GetObjectFromJson<List<BasketViewModel>>(this.HttpContext.Session, WebConstants.BasketKey);
                if (basketSession == null)
                {
                    basketSession = new List<BasketViewModel>();
                }

                var basketItem = basketSession.FirstOrDefault(x => x.Product.Id == id);
                if (basketItem == null)
                {
                    var product = await this.productsService.GetProductByIdAsync<ProductViewModel>(id);
                    basketItem = new BasketViewModel
                    {
                        Quantity = BasketItemInSessionDefaultQuantityValue,
                        Product = this.mapper.Map<ProductInBasketViewModel>(product)
                    };

                    basketSession.Add(basketItem);
                }

                basketItem.Quantity++;
                SessionHelper.SetObjectAsJson(HttpContext.Session, WebConstants.BasketKey, basketSession);
            }

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(EditBasketInputModel model)
        {
            //TODO: ask what value to return from this method! EditBasketItemViewModel is not very good
            if (this.User.Identity.IsAuthenticated)
            {
                var obj = await this.basketService.EditBasketItemAsync<EditBasketItemViewModel>(
                    model.BasketId,
                    model.ProductId, 
                    model.Quantity);
            }
            else
            {
                var basketSession = SessionHelper.GetObjectFromJson<List<BasketViewModel>>(
                    this.HttpContext.Session, 
                    WebConstants.BasketKey);

                var basketItem = basketSession.FirstOrDefault(x => x.Product.Id == model.ProductId);
                if (basketItem == null || model.Quantity <= 0)
                {
                    //TODO: throw error 
                }

                basketItem.Quantity = model.Quantity;
                SessionHelper.SetObjectAsJson(HttpContext.Session, WebConstants.BasketKey, basketSession);
            }

            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(DeleteBasketInputModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var isDeleted = await this.basketService.DeleteBasketItemAsync(model.BasketId, model.ProductId);
                if (!isDeleted)
                {
                    //TODO: throw error
                }
            }
            else
            {
                var basketSession = SessionHelper.GetObjectFromJson<List<BasketViewModel>>(this.HttpContext.Session, WebConstants.BasketKey);

                var basketItem = basketSession.FirstOrDefault(x => x.Product.Id == model.ProductId);
                if (basketItem != null)
                {
                    basketSession.Remove(basketItem);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, WebConstants.BasketKey, basketSession);
                }
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}