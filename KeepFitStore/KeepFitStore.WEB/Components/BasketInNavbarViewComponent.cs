namespace KeepFitStore.WEB.Components
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Helpers;
    using KeepFitStore.Models.ViewModels.Basket;
    using KeepFitStore.WEB.Common;

    [ViewComponentAttribute(Name = "BasketInNavbar")]
    public class BasketInNavbarViewComponent : ViewComponent
    {
        private readonly IBasketService basketService;

        public BasketInNavbarViewComponent(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<BasketViewModel> basketContent = new List<BasketViewModel>(); 

            if (this.User.Identity.IsAuthenticated)
            {
                 basketContent = await this.basketService.GetBasketContentAsync<BasketViewModel>(this.HttpContext.User);
            }
            else
            {
                basketContent = SessionHelper.GetObjectFromJson<List<BasketViewModel>>(this.HttpContext.Session, WebConstants.BasketKey);
            }

            return this.View(basketContent);
        }
    }
}