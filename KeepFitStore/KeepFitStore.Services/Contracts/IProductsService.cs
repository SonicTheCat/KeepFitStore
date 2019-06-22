namespace KeepFitStore.Services.Contracts
{
    using KeepFitStore.Models.Products;

    public interface IProductsService
    {
        void CreateProduct<TEntityType, TSourceType>(TSourceType sourceType)
            where TSourceType : class
            where TEntityType : class;
    }
}