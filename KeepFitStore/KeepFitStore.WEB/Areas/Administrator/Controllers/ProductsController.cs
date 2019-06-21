namespace KeepFitStore.WEB.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using AutoMapper;

    using KeepFitStore.Common;
    using KeepFitStore.Services.Contracts;
    using Areas.Administrator.Models.InputModels.Products;
    using KeepFitStore.Models.Products;

    [Area(GlobalConstants.AdministratorRoleName)]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class ProductsController : AdministratorController
    {
        private readonly IProductsService productsService;
        private readonly IMapper mapper;

        public ProductsController(IProductsService productsService, IMapper mapper)
        {
            this.productsService = productsService;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return this.View(); 
        }

        public IActionResult CreateProtein()
        {
            return this.View(); 
        }

        [HttpPost]
        public IActionResult CreateProtein(CreateProteinProductInputModel model)
        {
            var protein = this.mapper.Map<Protein>(model);
            this.productsService.CreateProtein(protein);

            return this.Redirect("/"); 
        }
    }
}