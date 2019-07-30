namespace KeepFitStore.WEB.Areas.Administrator.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.ViewModels.Products;

    public class ProductsController : AdministratorController
    {
        private readonly IProductsService productsSerive;

        public ProductsController(IProductsService productsSerive)
        {
            this.productsSerive = productsSerive;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await this.productsSerive.GetAll<IndexProductViewModel>();

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var deletedCount = await this.productsSerive.DeleteProductByIdAsync(id);

            return this.RedirectToAction(nameof(Index));
        }
    }
}