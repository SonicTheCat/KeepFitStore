namespace KeepFitStore.Services.Contracts
{
    using Microsoft.AspNetCore.Http;

    using KeepFitStore.Domain.Products;
    
    public interface IProductsService
    {
        void CreateProduct<TEntityType, TSourceType>(TSourceType sourceType, IFormFile image)
            where TSourceType : class
            where TEntityType : Product;
    }
}