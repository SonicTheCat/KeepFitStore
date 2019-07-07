using KeepFitStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeepFitStore.WEB.Components
{
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
            var basketContent = await this.basketService.GetBasketContentAsync(this.HttpContext.User);
            
            return this.View(basketContent);
        }
    }
}
