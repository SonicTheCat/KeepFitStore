namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    using KeepFitStore.Domain.Products;
    using KeepFitStore.Models.ViewModels.Products;

    public interface IProductsService
    {
        void CreateProduct<TEntityType, TSourceType>(TSourceType sourceType, IFormFile image)
            where TSourceType : class
            where TEntityType : Product;

        IEnumerable<ProductViewModel> AllProducts(); 
    }
}