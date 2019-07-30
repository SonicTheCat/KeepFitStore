namespace KeepFitStore.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using KeepFitStore.Domain.Products;
    using KeepFitStore.Helpers;

    public interface IProductsService
    {
        Task CreateProductAsync<TEntityType, TSourceType>(TSourceType sourceType, IFormFile image)
            where TSourceType : class
            where TEntityType : Product;

        Task<IEnumerable<TViewModel>> GetTopRatedProducts<TViewModel>(int countOfProducts);

        Task<IEnumerable<TViewModel>> GetNewestProductsAsync<TViewModel>(int countOfProducts);

        Task<PaginatedList<TViewModel>> SearchProductsAsync<TViewModel>(int pageNumber, int pageSize, string sortBy);

        Task<IEnumerable<TViewModel>> GetAll<TViewModel>();

        Task<TViewModel> GetProductByIdAsync<TViewModel>(int id);

        Task<int> DeleteProductByIdAsync(int id);

        Task<TDestination> FindProductForEditAsync<TDestination>(int id);

        Task<int> EditProductAsync<TDestination, TSourceType>(TSourceType model, IFormFile image, int productId)
            where TSourceType : class
            where TDestination : Product; 

        void ValidateProductType(Type enumType, string type);
    }
}