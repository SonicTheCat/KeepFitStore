namespace KeepFitStore.WEB.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.InputModels.Basket;
    using KeepFitStore.Models.ViewModels.Basket;
    using System.Collections.Generic;

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketApiController : ControllerBase
    {
        private readonly IBasketService basketService;

        public BasketApiController(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        [HttpGet]
        [ActionName(nameof(EditQuantity))]
        public async Task<ActionResult<EditBasketItemViewModel>> EditQuantity([FromQuery]EditBasketInputModel model)
        {
            var obj = await this.basketService.EditBasketItemAsync(model.BasketId, model.ProductId, model.Quantity);

            return obj;
        }


        [HttpGet]
        [ActionName(nameof(DeleteBasketItem))]
        public async Task DeleteBasketItem([FromQuery]DeleteBasketInputModel model)
        {
            var isDeleted = await this.basketService.DeleteBasketItemAsync(model.BasketId, model.ProductId);
            if (!isDeleted)
            {
                //throw error
            }
        }
    }
}