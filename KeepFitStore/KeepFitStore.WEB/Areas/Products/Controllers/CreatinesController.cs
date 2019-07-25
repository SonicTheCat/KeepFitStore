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

            this.ViewData["creatineType"] = type;
            var creatines = await this.creatinesService.GetAllByTypeAsync(type);

            return this.View(creatines);
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

        public async Task<IActionResult> Details(int id)
        {
            var creatine = await this.creatinesService.GetByIdAsync(id);

            //TODO: Write CustomException in services, they do not have to return null!
            if (creatine == null)
            {
                return this.NotFound();
            }

            return this.View(creatine);
        }
    }
}