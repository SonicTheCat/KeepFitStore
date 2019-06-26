namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    using KeepFitStore.Domain.Products;
    using KeepFitStore.Models.ViewModels.Products;
    using System.Threading.Tasks;

    public interface IProductsService
    {
        void CreateProduct<TEntityType, TSourceType>(TSourceType sourceType, IFormFile image)
            where TSourceType : class
            where TEntityType : Product;

        Task<IEnumerable<ProductViewModel>> GetTopRatedProducts();

        Task<IEnumerable<ProductViewModel>> GetNewestProductsAsync(); 
    }
}