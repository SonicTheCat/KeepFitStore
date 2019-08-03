namespace KeepFitStore.Services
{
    using System.Threading.Tasks;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Models.InputModels.Reviews;
    using KeepFitStore.Models.ViewModels.Products;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.CustomExceptions.Messsages;

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
                throw new ProductNotFoundException(string.Format(ExceptionMessages.ProductNotFound, model.ProductId));
            }

            var review = this.mapper.Map<Review>(model);

            this.context.Reviews.Add(review);
            await this.context.SaveChangesAsync(); 
        }
    }
}