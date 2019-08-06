namespace KeepFitStore.Services.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using Moq;
    using Xunit;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.Models.ViewModels.Basket;
    using KeepFitStore.Models.ViewModels.Products;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.Tests.Common;
    
    public class BasketServiceTests
    {
        private const string UserOneId = "123";
        private const string UserTwoId = "axxjajxa";

        private const string UserOneName = "admin@abv.bg";
        private const string UserTwoName = "unknown@abv.bg";

        private const int ProductId = 101;
        private const string Name = "productTest";
        private const decimal Price = 1.00m;
        private const string Description = "Very good";
        private const string ImageUrl = "url";
        private const int Rating = 4;

        private const int BasketId = 888;
        private const int LoopIterations = 10;

        private KeepFitDbContext context;
        private IMapper mapper;
        private IBasketService service;

        [Fact]
        public async Task AddProductToBasket_ShouldAdd()
        {
            this.Initialize();
            this.SeedUser();
            var mockedService = new Mock<IProductsService>();

            mockedService
               .Setup(x =>
                       x.GetProductByIdAsync<ProductViewModel>(ProductId))
               .Returns(Task.FromResult(GetTestProductData()));

            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            var initialCount = 0;
            Assert.Equal(initialCount, this.context.BasketItems.Count());

            await this.service.AddProductToBasketAsync(ProductId, UserOneName);

            var expectedCount = 1;
            Assert.Equal(expectedCount, this.context.BasketItems.Count());
        }

        [Fact]
        public async Task AddMultipleProductsWithSameIdToOneUser_ShoulCreateOnlyOneBasketItem()
        {
            this.Initialize();
            this.SeedUser();
            this.SeedProductsIDs();

            var mockedService = new Mock<IProductsService>();

            mockedService
               .Setup(x =>
                        x.GetProductByIdAsync<ProductViewModel>(ProductId))
               .Returns(Task.FromResult(GetTestProductData()));

            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            var quantityToAdd = 10;
            for (int i = 0; i < quantityToAdd; i++)
            {
                await this.service.AddProductToBasketAsync(ProductId, UserOneName);
                var expectedCount = 1;
                Assert.Equal(expectedCount, this.context.BasketItems.Count());
            }

            var basketItem = this.context.BasketItems.First();
            Assert.Equal(quantityToAdd, basketItem.Quantity);
        }

        [Fact]
        public async Task AddMultipleDiffrentProducts_ShoulCreatManyBasketItems()
        {
            this.Initialize();
            this.SeedUser();
            this.SeedProductsIDs();

            var mockedService = new Mock<IProductsService>();

            for (int i = 1; i < LoopIterations; i++)
            {
                var id = ProductId + i;

                mockedService
                   .Setup(x =>
                          x.GetProductByIdAsync<ProductViewModel>(id))
                   .Returns(Task.FromResult(GetTestProductData(id)));

                this.service = new BasketSerivce(context, mockedService.Object, mapper);

                await this.service.AddProductToBasketAsync(id, UserOneName);
                var expectedCount = i;
                Assert.Equal(expectedCount, this.context.BasketItems.Count());
            }

            var basketItems = this.context.BasketItems.ToList();
            var expectedQty = 1;
            foreach (var item in basketItems)
            {
                Assert.Equal(expectedQty, item.Quantity);
            }
        }

        [Fact]
        public async Task PassInvalidProductId_ShouldThrowProductExc()
        {
            this.Initialize();
            this.SeedUser();
            this.SeedProductsIDs();

            var mockedService = new Mock<IProductsService>();

            var invalidProductId = 555;

            mockedService
               .Setup(x =>
                      x.GetProductByIdAsync<ProductViewModel>(invalidProductId))
               .Returns(Task.FromResult((ProductViewModel)null));

            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            await Assert.ThrowsAsync<ProductNotFoundException>(() => this.service.AddProductToBasketAsync(invalidProductId, UserOneName));
        }

        [Fact]
        public async Task PassInvalidUsername_ShouldThrowUserExc()
        {
            this.Initialize();
            this.SeedUser();

            var mockedService = new Mock<IProductsService>();

            mockedService
              .Setup(x =>
                     x.GetProductByIdAsync<ProductViewModel>(ProductId))
              .Returns(Task.FromResult(GetTestProductData()));

            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            await Assert.ThrowsAsync<UserNotFoundException>(() => this.service.AddProductToBasketAsync(ProductId, UserTwoName));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(10, 10)]
        [InlineData(int.MaxValue, int.MaxValue)]
        public async Task EditBasketItem_ShouldWorkCorrect(int qtyToAdd, int expectedQty)
        {
            this.Initialize();
            this.SeedUser();
            this.SeedProductsIDs();
            this.SeedBasketItems();
            var mockedService = new Mock<IProductsService>();
            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            var viewModel = await this.service.EditBasketItemAsync<EditBasketItemViewModel>(BasketId, ProductId, qtyToAdd);
            Assert.Equal(expectedQty, viewModel.Quantity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public async Task EditBasketItemInvalidQty_ShouldThrow(int qtyToAdd)
        {
            this.Initialize();
            this.SeedUser();
            this.SeedProductsIDs();
            this.SeedBasketItems();
            var mockedService = new Mock<IProductsService>();
            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            await Assert.ThrowsAsync<InvalidQuantityProvidedException>(() => service.EditBasketItemAsync<EditBasketItemViewModel>(BasketId, ProductId, qtyToAdd));
        }

        [Fact]
        public async Task EditBasketItemInvalidBasketItem_ShouldThrow()
        {
            this.Initialize();
            var mockedService = new Mock<IProductsService>();
            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            var qty = 1;
            await Assert.ThrowsAsync<BasketItemNotFoundException>(() => service.EditBasketItemAsync<EditBasketItemViewModel>(BasketId, ProductId, qty));
        }

        [Fact]
        public async Task GetContentOnUserWhoHasNoProducts_ShouldReturnEmpty()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProductsIDs();
            this.SeedBasketItems();
            var mockedService = new Mock<IProductsService>();
            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            for (int i = 1; i < LoopIterations; i++)
            {
                var userWithNoProductsInBasket = UserOneName + i;
                var viewModel = await this.service.GetBasketContentAsync<BasketViewModel>(userWithNoProductsInBasket);

                Assert.Empty(viewModel);
            }
        }

        [Fact]
        public async Task GetContentFromFullBasket_ShouldRetunrFullCollection()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProductsIDs();
            this.SeedBasketItems();
            var mockedService = new Mock<IProductsService>();
            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            var viewModel = await this.service.GetBasketContentAsync<BasketViewModel>(UserOneName);

            var expectedCount = 10;
            var firstItem = viewModel.OrderBy(x => x.Product.Id).First();
            var lastItem = viewModel.OrderByDescending(x => x.Product.Id).First();
            var expectedIdOnLastItem = ProductId + (LoopIterations - 1);

            Assert.Equal(expectedCount, viewModel.Count());
            Assert.Equal(ProductId, firstItem.Product.Id);
            Assert.Equal(expectedIdOnLastItem, lastItem.Product.Id);
        }

        [Fact]
        public async Task GetContentWithInvalidUsername_ShouldThrowUserExc()
        {
            this.Initialize();
            this.SeedUser();
            var mockedService = new Mock<IProductsService>();
            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            await Assert.ThrowsAsync<UserNotFoundException>(() => this.service.GetBasketContentAsync<BasketViewModel>(UserTwoName));
        }

        [Theory]
        [InlineData(100.0, 1, 1000.0)]
        [InlineData(0.1, 10, 10.0)]
        [InlineData(10.10, 2, 202.0)]
        [InlineData(0.0, 1, 0.0)]
        public async Task GetBasketTotalPrice_ShouldWorkCorrect(decimal price, int qty, decimal expected)
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProductsIDs();
            this.SeedBasketItems();
            var mockedService = new Mock<IProductsService>();
            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            var basketitems = this.context
                .BasketItems
                .Include(x => x.Product)
                .ToList();

            foreach (var item in basketitems)
            {
                item.Product.Price = price;
                item.Quantity = qty;
            }
            this.context.SaveChanges();

            var total = await this.service.GetBasketTotalPriceAsync(UserOneName);
            Assert.Equal(expected, total);
        }

        [Fact]
        public async Task DeleteBasketItem_ShouldDelete()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProductsIDs();
            this.SeedBasketItems();
            var mockedService = new Mock<IProductsService>();
            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            var result = await this.service.DeleteBasketItemAsync(BasketId, ProductId);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteBasketItem_ShouldThrowBasketItemEsc()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProductsIDs();
            this.SeedBasketItems();
            var mockedService = new Mock<IProductsService>();
            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            await Assert.ThrowsAsync<BasketItemNotFoundException>(() => this.service.DeleteBasketItemAsync(BasketId, ProductId + 11));

            await Assert.ThrowsAsync<BasketItemNotFoundException>(() => this.service.DeleteBasketItemAsync(BasketId + 1, ProductId));
        }

        [Fact]
        public async Task ClearBasketContent_ShouldWork()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProductsIDs();
            this.SeedBasketItems();
            var mockedService = new Mock<IProductsService>();
            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            await this.service.ClearBasketAsync(BasketId);
            var expectedCount = 0;
            Assert.Equal(expectedCount, this.context.BasketItems.Count());
        }

        [Fact]
        public async Task ClearBasketContent_ShouldThrow()
        {
            this.Initialize();
            this.SeedUser();
            this.SeedProductsIDs();
            this.SeedBasketItems();
            var mockedService = new Mock<IProductsService>();
            this.service = new BasketSerivce(context, mockedService.Object, mapper);

            var id = BasketId + 1; 
            await Assert.ThrowsAsync<ServiceException>(() => this.service.ClearBasketAsync(id));
        }

        private void SeedBasketItems()
        {
            var user = this.context.Users.First(x => x.Id == UserOneId);

            for (int i = 0; i < LoopIterations; i++)
            {
                var basketItem = new BasketItem()
                {
                    BasketId = user.BasketId
                };

                basketItem.ProductId = i == 0 ? ProductId : ProductId + i;
                this.context.BasketItems.Add(basketItem);
            }

            this.context.SaveChanges();
        }

        private ProductViewModel GetTestProductData(int productId = ProductId)
        {
            return new ProductViewModel()
            {
                Id = productId,
                Name = Name,
                Price = Price,
                IsOnSale = false,
                Description = Description,
                ImageUrl = ImageUrl,
                Rating = Rating,
                ProductType = ProductType.Protein
            };
        }

        private void SeedProductsIDs()
        {
            for (int i = 0; i < LoopIterations; i++)
            {
                var product = new Protein();

                product.Id = i == 0 ? ProductId : ProductId + i;
                this.context.Products.Add(product);
            }

            this.context.SaveChanges();
        }

        private void SeedUser()
        {
            var user = new KeepFitUser()
            {
                Id = UserOneId,
                UserName = UserOneName,
                Basket = new Basket()
                {
                    Id = BasketId
                }
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        private void SeedUsers()
        {
            for (int i = 0; i < LoopIterations; i++)
            {
                var user = new KeepFitUser()
                {
                    Id = i == 0 ? UserOneId : UserOneId + i,
                    UserName = i == 0 ? UserOneName : UserOneName + i,
                    Basket = new Basket()
                    {
                        Id = i == 0 ? BasketId : BasketId + i
                    }
                };

                this.context.Users.Add(user);
            }

            this.context.SaveChanges();
        }

        private void Initialize()
        {
            this.context = KeepFitDbContextInMemoryFactory.Initialize();
            this.mapper = AutoMapperFactory.Initialize();
        }
    }
}