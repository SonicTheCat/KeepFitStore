using AutoMapper;
using KeepFitStore.Data;
using KeepFitStore.Domain.Enums;
using KeepFitStore.Domain.Products;
using KeepFitStore.Models.ViewModels.Products;
using KeepFitStore.Models.ViewModels.Products.Vitamins;
using KeepFitStore.Models.ViewModels.Reviews;
using KeepFitStore.Services.Contracts;
using KeepFitStore.Services.CustomExceptions;
using KeepFitStore.Services.Tests.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KeepFitStore.Services.Tests
{
    public class VitaminServiceTests
    {
        private const string Name = "proteinTest";
        private const decimal Price = 1.00m;
        private const string Description = "Very good";
        private const string Directions = "Take One";
        private const string ImageUrl = "url";

        private const int days = 100;
        private DateTime DateCreated = DateTime.UtcNow.AddDays(-days);

        private KeepFitDbContext context;
        private IMapper mapper;
        private IVitaminsService service;

        [Fact]
        public async Task GetVitaminById_ShouldWorkCorrect()
        {
            var startFrom = 1;
            var numberOfRowsToBeSeeded = 100;
            var wantedId = 10;

            this.Initialize();
            this.SeedVitamins(startFrom, numberOfRowsToBeSeeded, VitaminType.Multi);

            var expected = new DetailsVitaminViewModel()
            {
                Id = wantedId,
                Name = Name + wantedId,
                Price = Price + wantedId,
                Description = Description + wantedId,
                Directions = Directions + wantedId,
                ImageUrl = ImageUrl + wantedId,
                IsSuatableForVegans = false,
                ProductType = ProductType.Vitamin,
                Type = VitaminType.Multi,
                CreatedOn = DateCreated.AddDays(wantedId),
                Reviews = new HashSet<ReviewViewModel>()
            };

            var vitamin = await this.service.GetByIdAsync<DetailsVitaminViewModel>(wantedId);

            var obj1Str = JsonConvert.SerializeObject(expected);
            var obj2Str = JsonConvert.SerializeObject(vitamin);

            Assert.NotNull(vitamin);
            Assert.Equal(obj1Str, obj2Str);
        }

        [Fact]
        public async Task GetByInvalidId_ShouldThrow()
        {
            var startFrom = 1;
            var numberOfRowsToBeSeeded = 10;
            var wantedId = 21;

            this.Initialize();
            this.SeedVitamins(startFrom, numberOfRowsToBeSeeded, VitaminType.VitaminA);
            this.SeedVitamins(startFrom + numberOfRowsToBeSeeded, numberOfRowsToBeSeeded * 2, VitaminType.VitaminD);

            await Assert.ThrowsAsync<ProductNotFoundException>(() => service
           .GetByIdAsync<DetailsVitaminViewModel>(wantedId));
        }

        [Fact]
        public async Task GetAllByValidType_ShouldWorkCorrect()
        {
            var startFrom = 1;
            var numberOfRowsToBeSeeded = 3;
            var wantedType = VitaminType.VitaminE.ToString();

            this.Initialize();
            this.SeedVitamins(startFrom, numberOfRowsToBeSeeded, VitaminType.VitaminE);
            this.SeedVitamins(startFrom + numberOfRowsToBeSeeded, numberOfRowsToBeSeeded * 3, VitaminType.VitaminC);

            var vitamins = await this.service.GetAllByTypeAsync<ProductViewModel>(wantedType);

            var expectedCount = 3;
            Assert.Equal(expectedCount, vitamins.Count());

            var id = 1;
            Assert.Collection(vitamins,
                            item => Assert.Contains(Name + id++, item.Name),
                            item => Assert.Contains(Name + id++, item.Name),
                            item => Assert.Contains(Name + id++, item.Name));
        }

        [Fact]
        public async Task GetAllByValidType_WithNoData_ShouldReturnEmtpy()
        {
            var wantedType = VitaminType.Multi.ToString();

            this.Initialize();

            var vitamins = await this.service.GetAllByTypeAsync<ProductViewModel>(wantedType);

            Assert.Empty(vitamins);
        }

        [Fact]
        public async Task GetAllByInvalidType_ShouldThrow()
        {
            var startFrom = 1;
            var numberOfRowsToBeSeeded = 3;
            var wantedTypeInvalid = "InvalidType";

            this.Initialize();
            this.SeedVitamins(startFrom, numberOfRowsToBeSeeded, VitaminType.Multi);

            await Assert.ThrowsAsync<InvalidProductTypeException>(() => this.service.GetAllByTypeAsync<ProductViewModel>(wantedTypeInvalid));
        }

        private void SeedVitamins(int startFrom, int seedCount, VitaminType type)
        {
            var list = new List<Product>();

            for (int i = startFrom; i <= seedCount; i++)
            {
                var creatine = new Vitamin()
                {
                    Id = i,
                    Name = Name + i,
                    Price = Price + i,
                    Description = Description + i,
                    Directions = Directions + i,
                    ImageUrl = ImageUrl + i,
                    ProductType = ProductType.Vitamin,
                    IsSuatableForVegans = false,
                    Type = type,
                    CreatedOn = DateCreated.AddDays(i),
                };

                list.Add(creatine);
            }

            this.context.AddRange(list);
            this.context.SaveChanges();
        }

        private void Initialize()
        {
            this.context = KeepFitDbContextInMemoryFactory.Initialize();
            this.mapper = AutoMapperFactory.Initialize();
            this.service = new VitaminsService(context, mapper);
        }
    }
}
