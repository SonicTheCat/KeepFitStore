using KeepFitStore.Models.ViewModels.Products;
using KeepFitStore.Services.Contracts;
using KeepFitStore.WEB.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeepFitStore.WEB.Components
{
    [ViewComponentAttribute(Name = "MostOrdered")]
    public class MostOrderedProductsViewComponent : ViewComponent
    {
        private readonly IProductsService productsService;

        public MostOrderedProductsViewComponent(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await this.productsService
                .GetMostOrderedProducts<ProductViewModel>(WebConstants.CountOfProductsToBeShownOnHomePage);

            return this.View(products);
        }
    }
}
