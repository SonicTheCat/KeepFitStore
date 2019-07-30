namespace KeepFitStore.WEB.Components
{
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Common;
    using KeepFitStore.Models.ViewModels.Products;

    [ViewComponentAttribute(Name = "NewestProducts")]
    public class NewestProductsViewComponent : ViewComponent
    {
        private readonly IProductsService productsService;

        public NewestProductsViewComponent(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await this.productsService
                .GetNewestProductsAsync<ProductViewModel>(WebConstants.CountOfProductsToBeShownOnHomePage);

            return this.View(products);
        }
    }
}