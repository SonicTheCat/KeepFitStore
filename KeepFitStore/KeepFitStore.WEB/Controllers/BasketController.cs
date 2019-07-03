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

        public IActionResult Index()
        {
            return null;
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

            return null;
        }

        private int isExist(int id)
        {
            return 0;
        }
    }
}