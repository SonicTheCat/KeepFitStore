using AutoMapper;
using KeepFitStore.Data;
using KeepFitStore.Domain;
using KeepFitStore.Domain.Enums;
using KeepFitStore.Domain.Products;
using KeepFitStore.Models.ViewModels.Favorites;
using KeepFitStore.Services.Contracts;
using KeepFitStore.Services.CustomExceptions;
using KeepFitStore.Services.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KeepFitStore.Services.Tests
{
    public class FavoriteServiceTests
    {
        private const string UserOneId = "123";
        private const string UserTwoId = "axxjajxa";

        private const string UserOneName = "admin@abv.bg";
        private const string UserTwoName = "unknown@abv.bg";

        private const int ProductId = 101;
        private const string Name = "productTest";
        private const decimal Price = 1.00m;
        private const string ImageUrl = "url";

        private const int LoopIterations = 10;

        private KeepFitDbContext context;
        private IMapper mapper;
        private IFavoriteService service;

        [Fact]
        public async Task AddProductToFavorites_ShouldReturnTrue()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProductsIDs();

            var isAdded = await this.service.AddAsync(ProductId, UserOneName);
            Assert.True(isAdded);

            var expectedCount = 1;
            Assert.Equal(expectedCount, this.context.UserFavoriteProducts.Count());
        }

        [Fact]
        public async Task AddMultipleProductsToUserFavs_ShouldAddAndReturnTrue()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProductsIDs();

            for (int i = 1; i < LoopIterations; i++)
            {
                var isAdded = await this.service.AddAsync(ProductId + i, UserOneName);

                Assert.True(isAdded);
            }

            var expectedCount = LoopIterations - 1;
            Assert.Equal(expectedCount, this.context.UserFavoriteProducts.Count());
        }

        [Fact]
        public async Task AddProductWhichExistInFavs_ShouldReturnFalse()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProductsIDs();

            var isAdded = await this.service.AddAsync(ProductId, UserOneName);
            Assert.True(isAdded);

            isAdded = await this.service.AddAsync(ProductId, UserOneName);
            Assert.False(isAdded);

            var expectedCount = 1;
            Assert.Equal(expectedCount, this.context.UserFavoriteProducts.Count());
        }

        [Fact]
        public async Task TryAddWithInvalidUsername_ShouldThrowUserException()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProductsIDs();

            await Assert.ThrowsAsync<UserNotFoundException>(() => this.service.AddAsync(ProductId, UserTwoName));
        }

        [Fact]
        public async Task TryAddWithInvalidProductId_ShouldThrowProductException()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProductsIDs();

            await Assert.ThrowsAsync<ProductNotFoundException>(() => this.service.AddAsync(ProductId - 1, UserOneName));
        }

        [Fact]
        public async Task GetAll_ShouldReturnCorrectCollection()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedUserFavsWithProducts();

            var favs = await this.service.GetAllByUser<IndexFavoritesViewModel>(UserOneName);
            Assert.NotEmpty(favs);

            var expectedCount = 10;
            var expectedFirstProduct = new IndexFavoritesViewModel()
            {
                Id = ProductId + 0,
                Name = Name + 0,
                Price = Price + 0,
                ImageUrl = ImageUrl + 0,
                ProductType = ProductType.Protein,
                Rating = default
            };

            var expectedLastProduct = new IndexFavoritesViewModel()
            {
                Id = ProductId + 9,
                Name = Name + 9,
                Price = Price + 9,
                ImageUrl = ImageUrl + 9,
                ProductType = ProductType.Protein,
                Rating = default
            };

            var firstProduct = favs.OrderBy(x => x.Id).First();
            var lastProduct = favs.OrderByDescending(x => x.Id).First();

            var expFirstToStr = JsonConvert.SerializeObject(expectedFirstProduct);
            var actualFirstToStr = JsonConvert.SerializeObject(firstProduct);

            var expLastToStr = JsonConvert.SerializeObject(expectedLastProduct);
            var actualLastToStr = JsonConvert.SerializeObject(lastProduct);

            Assert.Equal(expectedCount, favs.Count());
            Assert.Equal(expFirstToStr, actualFirstToStr);
            Assert.Equal(expLastToStr, actualLastToStr);
        }

        [Fact]
        public async Task GetAllWithEmptyUserFavCollection_ShouldReturnEmptyCollection()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedUserFavsWithProducts();

            var favs = await this.service.GetAllByUser<IndexFavoritesViewModel>(UserTwoName);
            Assert.Empty(favs);
        }

        [Fact]
        public async Task RemoveWithValidData_ShouldReturnTrue()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedUserFavsWithProducts();

            var productId = ProductId + 0;
            var isRemoved = await this.service.RemoveAsync(productId, UserOneName);

            Assert.True(isRemoved);
        }

        [Fact]
        public async Task RemoveWithInvalidData_ShouldThrow()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedUserFavsWithProducts();

            var invalidId = 1; 
            await Assert.ThrowsAsync<ProductNotFoundException>(() => this.service.RemoveAsync(invalidId, UserOneName));
            await Assert.ThrowsAsync<ProductNotFoundException>(() => this.service.RemoveAsync(ProductId, UserTwoName));
        }

        private void SeedUserFavsWithProducts()
        {
            var user = this.context
                .Users
                .Include(x => x.FavoriteProducts)
                .First(x => x.Id == UserOneId);

            for (int i = 0; i < LoopIterations; i++)
            {
                Product product = new Protein()
                {
                    Id = ProductId + i,
                    Name = Name + i,
                    Price = Price + i,
                    ImageUrl = ImageUrl + i,
                    ProductType = ProductType.Protein
                };

                user.FavoriteProducts.Add(new KeepFitUserFavoriteProducts()
                {
                    Product = product
                });
            }

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
                };

                this.context.Users.Add(user);
            }

            this.context.SaveChanges();
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

        private void Initialize()
        {
            this.context = KeepFitDbContextInMemoryFactory.Initialize();
            this.mapper = AutoMapperFactory.Initialize();
            this.service = new FavoriteService(context, mapper);
        }
    }
}