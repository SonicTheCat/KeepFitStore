namespace KeepFitStore.Services.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Moq;

    using Xunit;

    using KeepFitStore.Data;
    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Models.InputModels.Reviews;
    using KeepFitStore.Models.ViewModels.Products;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.Tests.Common;
    using KeepFitStore.Domain;
    using Microsoft.EntityFrameworkCore;

    public class ReviewServiceTests
    {
        private const int ProductId = 1;
        private const string ProductName = "Booster";
        private const decimal Price = 49.80m;
        private const string Description = "Test Description!!!!!!";
        private const string ImageUrl = "Url";
        private const int Rating = 4;

        private const string Title = "Very nice!";
        private const string Content = "This is the best product ever....";
        private const int GivenRating = 5;

        private const string UserId = "123"; 

        private KeepFitDbContext context;
        private IMapper mapper;
        private IReviewsService service;

        [Fact]
        public async Task CreateAnonymousReview_ShouldCreateAndReturnCount()
        {
            this.Initialize();

            var input = new CreateReviewInputModel()
            {
                Title = Title,
                Content = Content,
                GivenRating = GivenRating,
                ProductId = ProductId
            };

            var expectedCount = 1;
            var actual = await this.service.CreateAsync(input);
            Assert.Equal(expectedCount, actual);
            Assert.Equal(expectedCount, this.context.Reviews.Count());
            await this.service.CreateAsync(input);
            Assert.Equal(2, this.context.Reviews.Count());
        }

        [Fact]
        public async Task CreateWithUser_ShouldCreateAndReturnCount()
        {
            this.Initialize();
            this.SeedUser(); 

            var input = new CreateReviewInputModel()
            {
                Title = Title,
                Content = Content,
                GivenRating = GivenRating,
                ProductId = ProductId,
                KeepFitUserId = UserId
            };

            var expectedCount = 1;
            var actual = await this.service.CreateAsync(input);
            var user = this.context.Users.Include(x => x.Reviews).First();
            var review = user.Reviews.First();
            Assert.Equal(expectedCount, actual);
            Assert.Equal(expectedCount, user.Reviews.Count); 
            Assert.Equal(Title, review.Title); 
            Assert.Equal(Content, review.Content); 
            Assert.Equal(ProductId, review.ProductId); 
        }

        private ProductViewModel GetMockedViewModel()
        {
            return new ProductViewModel()
            {
                Id = ProductId,
                Name = ProductName,
                Price = Price,
                IsOnSale = false,
                Description = Description,
                ImageUrl = ImageUrl,
                Rating = Rating,
                ProductType = ProductType.Amino
            };
        }

        private void SeedUser()
        {
            this.context.Users.Add(new KeepFitUser()
            {
                Id = UserId,
            });

            this.context.SaveChanges();
        }

        private void Initialize()
        {
            this.context = KeepFitDbContextInMemoryFactory.Initialize();
            this.mapper = AutoMapperFactory.Initialize();

            var productService = new Mock<IProductsService>();
            productService.Setup(x => x.GetProductByIdAsync<ProductViewModel>(ProductId))
                          .Returns(Task.FromResult(GetMockedViewModel()));

            this.service = new ReviewsService(context, productService.Object, mapper);
        }
    }
}