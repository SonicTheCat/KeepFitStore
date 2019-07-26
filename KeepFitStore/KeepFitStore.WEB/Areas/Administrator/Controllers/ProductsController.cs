namespace KeepFitStore.WEB.Areas.Administrator.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Services.Contracts;

    public class ProductsController : AdministratorController
    {
        private readonly IProductsService productsSerive;

        public ProductsController(IProductsService productsSerive)
        {
            this.productsSerive = productsSerive;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await this.productsSerive.GetAllAsync();

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }
    }
}