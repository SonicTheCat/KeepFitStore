namespace KeepFitStore.Services
{
    using KeepFitStore.Data;
    using KeepFitStore.Models.Products;
    using AutoMapper;
    using KeepFitStore.Services.Contracts;

    public class ProductsService : IProductsService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public ProductsService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void CreateProduct<TEntityType, TSourceType>(TSourceType sourceType)
            where TSourceType : class
            where TEntityType : class
        {
            var product = this.mapper.Map<TEntityType>(sourceType);

            if (product == null)
            {
                return;
            }

            this.context.Add(product);
            this.context.SaveChanges();
        }
    }
}