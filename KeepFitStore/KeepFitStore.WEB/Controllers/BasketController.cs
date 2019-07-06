using KeepFitStore.Domain;
using KeepFitStore.Domain.Products;
using KeepFitStore.Helpers;
using KeepFitStore.Models.ViewModels.Products;
using KeepFitStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeepFitStore.WEB.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketService basketService;

        public BasketController(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            // if (this.User.Identity.IsAuthenticated)
            //  {
            var basketContent = await this.basketService.GetBasketContentAsync(this.User);

            // }

            return this.View(basketContent);
        }

        public async Task<IActionResult> Add(int id)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                await this.basketService.AddProductToBasketAsync(id, this.User);
            }
            else
            {

            }

            return this.RedirectToAction(nameof(Index));
        }

       
        
    }
}