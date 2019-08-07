namespace KeepFitStore.WEB.Areas.Products.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Filters;
    using KeepFitStore.Models.InputModels.Products.Proteins;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.WEB.Common;
    using KeepFitStore.Models.ViewModels.Products;
    using KeepFitStore.Models.ViewModels.Products.Proteins;

    public class ProteinsController : ProductsController
    {
        private readonly IProductsService productsService;
        private readonly IProteinsService proteinsService;

        public ProteinsController(IProductsService productsService, IProteinsService proteinsService)
        {
            this.productsService = productsService;
            this.proteinsService = proteinsService;
        }

        public async Task<IActionResult> Index([FromQuery]string type)
        {
            if (type == null)
            {
                return this.Redirect(WebConstants.HomePagePath);
            }

            this.ViewData[WebConstants.ProteinType] = type;
            var proteins = await this.proteinsService.GetAllByTypeAsync<ProductViewModel>(type);

            return this.View(proteins);

        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        [ValidateModelStateFilter(nameof(Create))] //TODO:Refacotr ValidaModelStateFilter
        public async Task<IActionResult> Create(CreateProteinProductInputModel model)
        {
            await this.productsService.CreateProductAsync<Protein, CreateProteinProductInputModel>(model, model.Image);

            return this.Redirect(WebConstants.HomePagePath);
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(int id)
        {
            var protein = await this.productsService.GetProductByIdAsync<EditProteinProductInputModel>(id);
            return this.View(protein);
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(EditProteinProductInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(inputModel);
            }

            await this.productsService
               .EditProductAsync<Protein, EditProteinProductInputModel>(
               inputModel,
               inputModel.Image,
               inputModel.Id);

            return this.Redirect(WebConstants.AdministrationAllProductsPath);
        }

        public async Task<IActionResult> Details(int id)
        {
            var protein = await this.proteinsService.GetByIdAsync<DetailsProteinViewModel>(id);

            return this.View(protein);
        }
    }
}