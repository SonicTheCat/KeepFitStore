namespace KeepFitStore.WEB.Areas.Products.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Common;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Filters;
    using KeepFitStore.Models.InputModels.Products.Proteins;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.WEB.Common;

    public class ProteinsController : ProductsController
    {
        private readonly IProductsService productsService;

        public ProteinsController(IProductsService productsService)
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
        public async Task<IActionResult> Create(CreateProteinProductInputModel model)
        {
            await this.productsService.CreateProductAsync<Protein, CreateProteinProductInputModel>(model, model.Image);

            return this.Redirect(WebConstants.HomePagePath);
        }

        public async Task<IActionResult> Details(int id)
        {
            var protein = await this.productsService.GetProteinById(id);

            if (protein == null)
            {
                return this.NotFound(); 
            }

            return this.View(protein);
        }
    }
}