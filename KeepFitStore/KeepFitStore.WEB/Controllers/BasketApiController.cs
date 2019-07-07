namespace KeepFitStore.WEB.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.InputModels.Basket;
    using KeepFitStore.Models.ViewModels.Basket;
    using KeepFitStore.Helpers;
    using KeepFitStore.WEB.Common;

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketApiController : ControllerBase
    {
        private readonly IBasketService basketService;
        private readonly IProductsService productsService;

        public BasketApiController(IBasketService basketService, IProductsService productsService)
        {
            this.basketService = basketService;
            this.productsService = productsService;
        }

        [HttpGet]
        [ActionName(nameof(EditQuantity))]
        public async Task<ActionResult<EditBasketItemViewModel>> EditQuantity([FromQuery]EditBasketInputModel model)
        {
            //TODO: ask what value to return from this method! EditBasketItemViewModel is not very good
            if (this.User.Identity.IsAuthenticated)
            {
                var obj = await this.basketService.EditBasketItemAsync(model.BasketId, model.ProductId, model.Quantity);

                return obj;
            }
            else
            {
                var basketSession = SessionHelper.GetObjectFromJson<List<BasketViewModel>>(this.HttpContext.Session, WebConstants.BasketKey);

                var basketItem = basketSession.FirstOrDefault(x => x.Product.Id == model.ProductId);
                if (basketItem == null || model.Quantity <= 0)
                {
                   //TODO: throw error 
                }

                basketItem.Quantity = model.Quantity;
                SessionHelper.SetObjectAsJson(HttpContext.Session, WebConstants.BasketKey, basketSession);
                var product = await this.productsService.GetProductByIdAsync(model.ProductId);
                return new EditBasketItemViewModel
                {
                    Quantity = model.Quantity,
                    ProductPrice = product.Price
                };
            }
        }

        [HttpGet]
        [ActionName(nameof(DeleteBasketItem))]
        public async Task DeleteBasketItem([FromQuery]DeleteBasketInputModel model)
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
        }
    }
}