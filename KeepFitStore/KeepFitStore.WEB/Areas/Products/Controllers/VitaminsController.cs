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
    using KeepFitStore.Models.ViewModels.Products.Vitamins;
    using KeepFitStore.Models.ViewModels.Products;

    public class VitaminsController : ProductsController
    {
        private readonly IProductsService productsService;
        private readonly IVitaminsService vitaminsService;

        public VitaminsController(IProductsService productsService, IVitaminsService vitaminsService)
        {
            this.productsService = productsService;
            this.vitaminsService = vitaminsService;
        }

        public async Task<IActionResult> Index([FromQuery]string type)
        {
            if (type == null)
            {
                return this.Redirect(WebConstants.HomePagePath);
            }

            this.ViewData[WebConstants.VitaminType] = type;
            var vitamins = await this.vitaminsService.GetAllByTypeAsync<ProductViewModel>(type);

            return this.View(vitamins);
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        [ValidateModelStateFilter(nameof(Create))]
        public async Task<IActionResult> Create(CreateVitaminProductInputModel model)
        {
            await this.productsService.CreateProductAsync<Vitamin, CreateVitaminProductInputModel>(model, model.Image);

            return this.Redirect(WebConstants.HomePagePath);
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(int id)
        {
            var protein = await this.productsService.GetProductByIdAsync<EditVitaminProductInputModel>(id);
            return this.View(protein);
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(EditVitaminProductInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(inputModel);
            }

            var protein = await this.productsService
                .EditProductAsync<Vitamin, EditVitaminProductInputModel>(
                inputModel,
                inputModel.Image,
                inputModel.Id);

            return this.Redirect(WebConstants.AdministrationAllProductsPath);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vitamin = await this.vitaminsService.GetByIdAsync<DetailsVitaminViewModel>(id);

            return this.View(vitamin);
        }
    }
}