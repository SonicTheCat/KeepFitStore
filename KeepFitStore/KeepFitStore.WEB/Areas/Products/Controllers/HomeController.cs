namespace KeepFitStore.WEB.Areas.Products.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Common;

    public class HomeController : ProductsController
    {
        private readonly IProductsService productsSerive;

        public HomeController(IProductsService productsSerive)
        {
            this.productsSerive = productsSerive;
        }

        public async Task<IActionResult> Index(
            int pageNumber = WebConstants.DefaultPageNumber, 
            int pageSize = WebConstants.DefaultPageSize,
            string sortBy = WebConstants.DefaultSorting)
        {
            var viewModel = await this.productsSerive.SearchProductsAsync(pageNumber, pageSize, sortBy);

            return this.View(viewModel);
        }
    }
}