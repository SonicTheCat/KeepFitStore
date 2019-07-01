namespace KeepFitStore.WEB.Areas.Products.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Filters;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.WEB.Common;
    using KeepFitStore.Models.InputModels.Products.Vitamins;

    public class VitaminsController : ProductsController
    {
        private readonly IProductsService productsService;
        private readonly IVitaminsService vitaminsService;

        public VitaminsController(IProductsService productsService, IVitaminsService vitaminsService)
        {
            this.productsService = productsService;
            this.vitaminsService = vitaminsService;
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

        public async Task<IActionResult> Details(int id)
        {
            var vitamin = await this.vitaminsService.GetByIdAsync(id);

            //TODO: Write CustomException in services, they do not have to return null!
            if (vitamin == null)
            {
                return this.NotFound();
            }

            return this.View(vitamin);
        }
    }
}