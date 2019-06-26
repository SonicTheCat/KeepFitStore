namespace KeepFitStore.WEB.Components
{
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Common;

    [ViewComponentAttribute(Name = "TopRatedProducts")]
    public class TopRatedProductsViewComponent : ViewComponent
    {
        private readonly IProductsService productsService;

        public TopRatedProductsViewComponent(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await this.productsService.GetTopRatedProducts(WebConstants.CountOfProductsToBeShownOnHomePage);

            return this.View(products);
        }
    }
}