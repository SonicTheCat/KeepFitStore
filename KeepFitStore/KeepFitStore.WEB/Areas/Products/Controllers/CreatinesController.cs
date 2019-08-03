namespace KeepFitStore.WEB.Areas.Products.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Filters;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.WEB.Common;
    using KeepFitStore.Models.InputModels.Products.Creatines;
    using KeepFitStore.Models.ViewModels.Products.Creatines;
    using KeepFitStore.Models.ViewModels.Products;

    public class CreatinesController : ProductsController
    {
        private readonly IProductsService productsService;
        private readonly ICreatinesService creatinesService;

        public CreatinesController(IProductsService productsService, ICreatinesService creatinesService)
        {
            this.productsService = productsService;
            this.creatinesService = creatinesService;
        }

        public async Task<IActionResult> Index([FromQuery]string type)
        {
            if (type == null)
            {
                return this.Redirect(WebConstants.HomePagePath);
            }

            this.ViewData[WebConstants.CreatineType] = type;
            var creatines = await this.creatinesService.GetAllByTypeAsync<ProductViewModel>(type);

            return this.View(creatines);
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        [ValidateModelStateFilter(nameof(Create))]
        public async Task<IActionResult> Create(CreateCreatineProductInputModel model)
        {
            await this.productsService.CreateProductAsync<Creatine, CreateCreatineProductInputModel>(model, model.Image);

            return this.Redirect(WebConstants.HomePagePath);
        }

        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(int id)
        {
            var protein = await this.productsService.FindProductForEditAsync<EditCreatineProductInputModel>(id);
            return this.View(protein);
        }

        [HttpPost]
        [Authorize(Roles = WebConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(EditCreatineProductInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(inputModel);
            }

            var protein = await this.productsService
                .EditProductAsync<Creatine, EditCreatineProductInputModel>(
                inputModel,
                inputModel.Image,
                inputModel.Id);

            return this.Redirect(WebConstants.AdministrationAllProductsPath);
        }

        public async Task<IActionResult> Details(int id)
        {
            var creatine = await this.creatinesService.GetByIdAsync<DetailsCreatineViewModel>(id);

            return this.View(creatine);
        }
    }
}