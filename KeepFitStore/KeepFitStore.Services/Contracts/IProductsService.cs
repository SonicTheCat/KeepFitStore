namespace KeepFitStore.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using KeepFitStore.Domain.Products;
    using KeepFitStore.Models.ViewModels.Products;

    public interface IProductsService
    {
        Task CreateProductAsync<TEntityType, TSourceType>(TSourceType sourceType, IFormFile image)
            where TSourceType : class
            where TEntityType : Product;

        Task<IEnumerable<ProductViewModel>> GetTopRatedProducts(int countOfProducts);

        Task<IEnumerable<ProductViewModel>> GetNewestProductsAsync(int countOfProducts);

        Task<IEnumerable<ProductViewModel>> GetAllWithReviews();

        Task<IEnumerable<IndexProductViewModel>> GetAll();

        Task<ProductViewModel> GetProductByIdAsync(int id);

        Task<int> DeleteProductByIdAsync(int id);

        Task<TDestination> FindProductForEditAsync<TDestination>(int id);

        Task<int> EditProductAsync<TDestination, TSourceType>(TSourceType model, IFormFile image, int productId)
            where TSourceType : class
            where TDestination : Product; 

        void ValidateProductType(Type enumType, string type);
    }
}