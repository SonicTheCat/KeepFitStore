namespace KeepFitStore.WEB.Components
{
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    using KeepFitStore.Services.Contracts;
    
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
            var products = await this.productsService.GetNewestProductsAsync();

            return this.View(products);
        }
    }
}