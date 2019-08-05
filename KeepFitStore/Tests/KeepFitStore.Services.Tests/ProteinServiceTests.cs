using AutoMapper;
using KeepFitStore.Data;
using KeepFitStore.Domain.Enums;
using KeepFitStore.Domain.Products;
using KeepFitStore.Models.ViewModels.Products;
using KeepFitStore.Models.ViewModels.Products.Proteins;
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
    public class ProteinServiceTests
    {
        private const string Name = "proteinTest";
        private const decimal Price = 1.00m;
        private const string Description = "Very good";
        private const string Directions = "Take One";
        private const string ImageUrl = "url";

        private const double EnergyPerServing = 100;
        private const double Carbohydrate = 2;
        private const double Fat = 3;
        private const double ProteinPerServing = 4;
        private const double Salt = 5;
        private const double Fibre = 6;

        private const int days = 100;
        private DateTime DateCreated = DateTime.UtcNow.AddDays(-days);

        private KeepFitDbContext context;
        private IMapper mapper;
        private IProteinsService service;

        [Fact]
        public async Task GetProteinById_ShouldWorkCorrect()
        {
            var startFrom = 1;
            var numberOfAminosToBeSeeded = 100;
            var wantedId = 99;

            this.Initialize();
            this.SeedProteins(startFrom, numberOfAminosToBeSeeded, ProteinType.Whey);

            var expected = new DetailsProteinViewModel()
            {
                Id = wantedId,
                Name = Name + wantedId,
                Price = Price + wantedId,
                Description = Description + wantedId,
                Directions = Directions + wantedId,
                ImageUrl = ImageUrl + wantedId,
                IsSuatableForVegans = false,
                ProductType = ProductType.Protein,
                Type = ProteinType.Whey,
                CreatedOn = DateCreated.AddDays(wantedId),
                EnergyPerServing = EnergyPerServing + wantedId,
                Carbohydrate = Carbohydrate + wantedId,
                Fat = Fat + wantedId,
                ProteinPerServing = ProteinPerServing + wantedId,
                Salt = Salt + wantedId,
                Fibre = Fibre + wantedId,
                Reviews = new HashSet<ReviewViewModel>()
            };

            var protein = await this.service.GetByIdAsync<DetailsProteinViewModel>(wantedId);

            var obj1Str = JsonConvert.SerializeObject(expected);
            var obj2Str = JsonConvert.SerializeObject(protein);

            Assert.NotNull(protein);
            Assert.Equal(obj1Str, obj2Str);
        }

        [Fact]
        public async Task GetAminoByInvalidId_ShouldThrow()
        {
            var startFrom = 1;
            var numberOfAminosToBeSeeded = 10;
            var wantedId = 0;

            this.Initialize();
            this.SeedProteins(startFrom, numberOfAminosToBeSeeded, ProteinType.Whey);

            await Assert.ThrowsAsync<ProductNotFoundException>(() => service
           .GetByIdAsync<DetailsProteinViewModel>(wantedId));
        }

        [Fact]
        public async Task GetAllByValidType_ShouldWorkCorrect()
        {
            var startFrom = 1;
            var numberOfAminosToBeSeeded = 3;
            var wantedType = ProteinType.Diet.ToString();

            this.Initialize();
            this.SeedProteins(startFrom, numberOfAminosToBeSeeded, ProteinType.Vegan);
            this.SeedProteins(startFrom + numberOfAminosToBeSeeded, numberOfAminosToBeSeeded * 3, ProteinType.Diet);

            var proteins = await this.service.GetAllByTypeAsync<ProductViewModel>(wantedType);

            var expectedCount = 6;
            Assert.Equal(expectedCount, proteins.Count());

            var id = 4;
            Assert.Collection(proteins,
                            item => Assert.Contains(Name + id++, item.Name),
                            item => Assert.Contains(Name + id++, item.Name),
                            item => Assert.Contains(Name + id++, item.Name),
                            item => Assert.Contains(Name + id++, item.Name),
                            item => Assert.Contains(Name + id++, item.Name),
                            item => Assert.Contains(Name + id++, item.Name));
        }

        [Fact]
        public async Task GetAllByValidType_WithNoData_ShouldReturnEmtpy()
        {
            var wantedType = ProteinType.Vegan.ToString();

            this.Initialize();

            var proteins = await this.service.GetAllByTypeAsync<ProductViewModel>(wantedType);

            Assert.Empty(proteins);
        }

        [Fact]
        public async Task GetAllByInvalidType_ShouldThrow()
        {
            var startFrom = 1;
            var numberOfAminosToBeSeeded = 3;
            var wantedTypeInvalid = "InvalidType";

            this.Initialize();
            this.SeedProteins(startFrom, numberOfAminosToBeSeeded, ProteinType.Casein);

            await Assert.ThrowsAsync<InvalidProductTypeException>(() => this.service.GetAllByTypeAsync<ProductViewModel>(wantedTypeInvalid));
        }

        private void SeedProteins(int startFrom, int seedCount, ProteinType type)
        {
            var list = new List<Product>();

            for (int i = startFrom; i <= seedCount; i++)
            {
                var protein = new Protein()
                {
                    Id = i,
                    Name = Name + i,
                    Price = Price + i,
                    Description = Description + i,
                    Directions = Directions + i,
                    ImageUrl = ImageUrl + i,
                    ProductType = ProductType.Protein,
                    IsSuatableForVegans = false,
                    Type = type,
                    CreatedOn = DateCreated.AddDays(i),
                    EnergyPerServing = EnergyPerServing + i,
                    Carbohydrate = Carbohydrate + i,
                    Fat = Fat + i,
                    ProteinPerServing = ProteinPerServing + i,
                    Salt = Salt + i,
                    Fibre = Fibre + i
                };

                list.Add(protein);
            }

            this.context.AddRange(list);
            this.context.SaveChanges();
        }

        private void Initialize()
        {
            this.context = KeepFitDbContextInMemoryFactory.Initialize();
            this.mapper = AutoMapperFactory.Initialize();
            this.service = new ProteinsService(context, mapper);
        }
    }
}