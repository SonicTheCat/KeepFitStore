namespace KeepFitStore.WEB.Areas.Products.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Filters;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.WEB.Common;
    using KeepFitStore.Models.InputModels.Products.Aminos;
    using KeepFitStore.Models.ViewModels.Products.Aminos;
    using KeepFitStore.Models.ViewModels.Products;

    public class AminosController : ProductsController
    {
        private readonly IProductsService productsService;
        private readonly IAminosService aminosService;

        public AminosController(IProductsService productsService, IAminosService aminosService)
        {
            this.productsService = productsService;
            this.aminosService = aminosService;
        }

        public async Task<IActionResult> Index([FromQuery]string type)
        {
            if (type == null)
            {
                return this.Redirect(WebConstants.HomePagePath);
            }

            this.ViewData[WebConstants.AminoType] = type;

            var aminos = await this.aminosService.GetAllByTypeAsync<ProductViewModel>(type);
            return this.View(aminos);
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        [ValidateModelStateFilter(nameof(Create))]
        public async Task<IActionResult> Create(CreateAminoAcidProducInputModel model)
        {
            await this.productsService.CreateProductAsync<AminoAcid, CreateAminoAcidProducInputModel>(model, model.Image);

            return this.Redirect(WebConstants.HomePagePath);
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(int id)
        {
            var protein = await this.productsService.FindProductForEditAsync<EditAminoProductInputModel>(id);
            return this.View(protein);
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(EditAminoProductInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(inputModel);
            }

            var protein = await this.productsService
                .EditProductAsync<AminoAcid, EditAminoProductInputModel>(
                inputModel,
                inputModel.Image,
                inputModel.Id);

            return this.Redirect(WebConstants.AdministrationAllProductsPath);
        }

        public async Task<IActionResult> Details(int id)
        {
            var amino = await this.aminosService.GetByIdAsync<DetailsAminoViewModel>(id);

            return this.View(amino);
        }
    }
}