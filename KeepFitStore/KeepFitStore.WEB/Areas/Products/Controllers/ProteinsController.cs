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
            this.ViewData["proteinType"] = type;
            var proteins = await this.proteinsService.GetAllByTypeAsync(type);

            return this.View(proteins); 

        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [ValidateModelStateFilter(nameof(Create))]
        public async Task<IActionResult> Create(CreateProteinProductInputModel model)
        {
            await this.productsService.CreateProductAsync<Protein, CreateProteinProductInputModel>(model, model.Image);

            return this.Redirect(WebConstants.HomePagePath);
        }

        public async Task<IActionResult> Details(int id)
        {
            var protein = await this.proteinsService.GetByIdAsync(id);

            //TODO: Write CustomException in services, they do not have to return null!
            if (protein == null)
            {
                return this.NotFound();
            }

            return this.View(protein);
        }
    }
}