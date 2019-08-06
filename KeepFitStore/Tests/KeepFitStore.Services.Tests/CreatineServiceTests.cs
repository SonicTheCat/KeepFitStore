namespace KeepFitStore.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Newtonsoft.Json;

    using Xunit;

    using KeepFitStore.Data;
    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.Models.ViewModels.Products;
    using KeepFitStore.Models.ViewModels.Products.Creatines;
    using KeepFitStore.Models.ViewModels.Reviews;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.Tests.Common;

    public class CreatineServiceTests
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
        private ICreatinesService service;

        [Fact]
        public async Task GetCreatineById_ShouldWorkCorrect()
        {
            var startFrom = 1;
            var numberOfRowsToBeSeeded = 100;
            var wantedId = 10;

            this.Initialize();
            this.SeedCreatines(startFrom, numberOfRowsToBeSeeded, CreatineType.Capsules);

            var expected = new DetailsCreatineViewModel()
            {
                Id = wantedId,
                Name = Name + wantedId,
                Price = Price + wantedId,
                Description = Description + wantedId,
                Directions = Directions + wantedId,
                ImageUrl = ImageUrl + wantedId,
                IsSuatableForVegans = false,
                ProductType = ProductType.Creatine,
                Type = CreatineType.Capsules,
                CreatedOn = DateCreated.AddDays(wantedId),
                Reviews = new HashSet<ReviewViewModel>()
            };

            var creatine = await this.service.GetByIdAsync<DetailsCreatineViewModel>(wantedId);

            var obj1Str = JsonConvert.SerializeObject(expected);
            var obj2Str = JsonConvert.SerializeObject(creatine);

            Assert.NotNull(creatine);
            Assert.Equal(obj1Str, obj2Str);
        }

        [Fact]
        public async Task GetByInvalidId_ShouldThrow()
        {
            var startFrom = 1;
            var numberOfRowsToBeSeeded = 10;
            var wantedId = 21;

            this.Initialize();
            this.SeedCreatines(startFrom, numberOfRowsToBeSeeded, CreatineType.Powder);
            this.SeedCreatines(startFrom + numberOfRowsToBeSeeded, numberOfRowsToBeSeeded * 2, CreatineType.Capsules);

            await Assert.ThrowsAsync<ProductNotFoundException>(() => service
           .GetByIdAsync<DetailsCreatineViewModel>(wantedId));
        }

        [Fact]
        public async Task GetAllByValidType_ShouldWorkCorrect()
        {
            var startFrom = 1;
            var numberOfRowsToBeSeeded = 3;
            var wantedType = CreatineType.Powder.ToString();

            this.Initialize();
            this.SeedCreatines(startFrom, numberOfRowsToBeSeeded, CreatineType.Powder);
            this.SeedCreatines(startFrom + numberOfRowsToBeSeeded, numberOfRowsToBeSeeded * 3, CreatineType.Capsules);

            var creatine = await this.service.GetAllByTypeAsync<ProductViewModel>(wantedType);

            var expectedCount = 3;
            Assert.Equal(expectedCount, creatine.Count());

            var id = 1;
            Assert.Collection(creatine,
                            item => Assert.Contains(Name + id++, item.Name),
                            item => Assert.Contains(Name + id++, item.Name),
                            item => Assert.Contains(Name + id++, item.Name));
        }

        [Fact]
        public async Task GetAllByValidType_WithNoData_ShouldReturnEmtpy()
        {
            var wantedType = CreatineType.Capsules.ToString();

            this.Initialize();

            var proteins = await this.service.GetAllByTypeAsync<ProductViewModel>(wantedType);

            Assert.Empty(proteins);
        }

        [Fact]
        public async Task GetAllByInvalidType_ShouldThrow()
        {
            var startFrom = 1;
            var numberOfRowsToBeSeeded = 3;
            var wantedTypeInvalid = "InvalidType";

            this.Initialize();
            this.SeedCreatines(startFrom, numberOfRowsToBeSeeded, CreatineType.Capsules);

            await Assert.ThrowsAsync<InvalidProductTypeException>(() => this.service.GetAllByTypeAsync<ProductViewModel>(wantedTypeInvalid));
        }

        private void SeedCreatines(int startFrom, int seedCount, CreatineType type)
        {
            var list = new List<Product>();

            for (int i = startFrom; i <= seedCount; i++)
            {
                var creatine = new Creatine()
                {
                    Id = i,
                    Name = Name + i,
                    Price = Price + i,
                    Description = Description + i,
                    Directions = Directions + i,
                    ImageUrl = ImageUrl + i,
                    ProductType = ProductType.Creatine,
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
            this.service = new CreatinesService(context, mapper);
        }
    }
}