namespace KeepFitStore.WEB.Areas.Products.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Common;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Filters;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.WEB.Common;
    using KeepFitStore.Models.InputModels.Products.Vitamins;

    public class VitaminsController : ProductsController
    {
        private readonly IProductsService productsService;

        public VitaminsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [ValidateModelStateFilter(nameof(Create))]
        public async Task<IActionResult> Create(CreateVitaminProductInputModel model)
        {
            await this.productsService.CreateProductAsync<Vitamin, CreateVitaminProductInputModel>(model, model.Image);

            return this.Redirect(WebConstants.HomePagePath);
        }
    }
}