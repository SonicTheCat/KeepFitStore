namespace KeepFitStore.Services.Contracts
{
    using KeepFitStore.Models.Products;

    public interface IProductsService
    {
        void CreateProtein(Protein protein); 
    }
}