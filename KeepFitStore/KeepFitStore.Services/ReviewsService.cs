namespace KeepFitStore.Services
{
    using System.Threading.Tasks;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Models.InputModels.Reviews;
    using KeepFitStore.Models.ViewModels.Products;
    using KeepFitStore.Services.Contracts;

    public class ReviewsService : IReviewsService
    {
        private readonly KeepFitDbContext context;
        private readonly IProductsService productsService;
        private readonly IMapper mapper;

        public ReviewsService(KeepFitDbContext context, IProductsService productsService, IMapper mapper)
        {
            this.context = context;
            this.productsService = productsService;
            this.mapper = mapper;
        }

        public async Task CreateAsync(CreateReviewInputModel model)
        {
            var product = await this.productsService.GetProductByIdAsync<ProductViewModel>(model.ProductId);

            if (product == null)
            {
                //TODO: throw service error
            }

            var review = this.mapper.Map<Review>(model);

            this.context.Reviews.Add(review);
            await this.context.SaveChangesAsync(); 
        }
    }
}