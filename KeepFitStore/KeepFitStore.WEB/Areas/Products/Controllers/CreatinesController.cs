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
    using KeepFitStore.Models.InputModels.Products.Creatines;

    public class CreatinesController : ProductsController
    {
        private readonly IProductsService productsService;

        public CreatinesController(IProductsService productsService)
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
        public async Task<IActionResult> Create(CreateCreatineProductInputModel model)
        {
            await this.productsService.CreateProductAsync<Creatine, CreateCreatineProductInputModel>(model, model.Image);

            return this.Redirect(WebConstants.HomePagePath);
        }
    }
}