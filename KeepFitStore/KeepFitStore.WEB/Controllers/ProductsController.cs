using KeepFitStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace KeepFitStore.WEB.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public IActionResult Details(int id)
        {
            //this.productsService.ProductDetails(id); 

            return this.View(); 
        }
    }
}