namespace KeepFitStore.WEB.Areas.Products.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Services.Contracts;

    public class HomeController : ProductsController
    {
        private readonly IProductsService productsSerive;

        public HomeController(IProductsService productsSerive)
        {
            this.productsSerive = productsSerive;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await this.productsSerive.GetAllWithReviews();

            return this.View(viewModel);
        }
    }
}