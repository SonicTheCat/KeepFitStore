namespace KeepFitStore.Services
{
    using KeepFitStore.Data;
    using KeepFitStore.Models.Products;
    using KeepFitStore.Services.Contracts;

    public class ProductsService : IProductsService
    {
        private readonly KeepFitDbContext context;

        public ProductsService(KeepFitDbContext context)
        {
            this.context = context;
        }

        public void CreateProtein(Protein protein)
        {
            if (protein == null)
            {
                return; 
            }

            this.context.Proteins.Add(protein); 
            this.context.SaveChanges();
        }
    }
}