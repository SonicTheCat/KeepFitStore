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

            this.ViewData["aminoType"] = type;
            var aminos = await this.aminosService.GetAllByTypeAsync(type);

            return this.View(aminos);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [ValidateModelStateFilter(nameof(Create))]
        public async Task<IActionResult> Create(CreateAminoAcidProducInputModel model)
        {
            await this.productsService.CreateProductAsync<AminoAcid, CreateAminoAcidProducInputModel>(model, model.Image);

            return this.Redirect(WebConstants.HomePagePath);
        }

        public async Task<IActionResult> Details(int id)
        {
            var amino = await this.aminosService.GetByIdAsync(id);

            //TODO: Write CustomException in services, they do not have to return null!
            if (amino == null)
            {
                return this.NotFound();
            }

            return this.View(amino);
        }
    }
}