namespace KeepFitStore.WEB.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Common;
    using KeepFitStore.Services.Contracts;
    using Areas.Administrator.Models.InputModels.Products;
    using KeepFitStore.Models.Products;
    using KeepFitStore.WEB.Common;
    using KeepFitStore.WEB.Filters;

    [Area(GlobalConstants.AdministratorRoleName)]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class ProductsController : AdministratorController
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        public IActionResult CreateProtein()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateModelStateFilter(nameof(CreateProtein))]
        public IActionResult CreateProtein(CreateProteinProductInputModel model)
        {
            this.productsService.CreateProduct<Protein, CreateProteinProductInputModel>(model);

            return this.Redirect(WebConstants.HomePagePath);
        }

        public IActionResult CreateCreatine()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateModelStateFilter(nameof(CreateCreatine))]
        public IActionResult CreateCreatine(CreateCreatineProductInputModel model)
        {
            this.productsService.CreateProduct<Creatine, CreateCreatineProductInputModel>(model);

            return this.Redirect(WebConstants.HomePagePath);
        }
    }
}