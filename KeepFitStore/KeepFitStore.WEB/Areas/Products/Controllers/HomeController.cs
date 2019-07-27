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

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 18)
        {
            var viewModel = await this.productsSerive.GetAllWithReviews(pageNumber, pageSize);

            return this.View(viewModel);
        }
    }
}