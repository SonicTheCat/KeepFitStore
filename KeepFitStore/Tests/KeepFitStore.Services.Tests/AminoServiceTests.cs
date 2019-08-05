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
    using KeepFitStore.Models.ViewModels.Products.Aminos;
    using KeepFitStore.Models.ViewModels.Reviews;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.Tests.Common;

    public class AminoServiceTests
    {
        private const string Name = "aminoTest";
        private const decimal Price = 1.00m;
        private const string Description = "Very good";
        private const string Directions = "Take One";
        private const string ImageUrl = "url";

        private const double EnergyPerServing = 100;
        private const double Carbohydrate = 2;
        private const double Fat = 3;
        private const double ProteinPerServing = 4;
        private const double Salt = 5;

        private const int days = 100;
        private DateTime DateCreated = DateTime.UtcNow.AddDays(-days);

        private KeepFitDbContext context;
        private IMapper mapper;
        private IAminosService service;

        [Fact]
        public async Task GetAminoById_ShouldWorkCorrect()
        {
            var startFrom = 1;
            var numberOfAminosToBeSeeded = 10;
            var wantedId = 1;

            this.Initialize();
            this.SeedAminos(startFrom, numberOfAminosToBeSeeded, AminoAcidType.BCAA);

            var expected = new DetailsAminoViewModel()
            {
                Id = wantedId,
                Name = Name + wantedId,
                Price = Price + wantedId,
                Description = Description + wantedId,
                Directions = Directions + wantedId,
                ImageUrl = ImageUrl + wantedId,
                IsSuatableForVegans = false,
                ProductType = ProductType.Amino,
                Type = AminoAcidType.BCAA,
                CreatedOn = DateCreated.AddDays(wantedId),
                EnergyPerServing = EnergyPerServing + wantedId,
                Carbohydrate = Carbohydrate + wantedId,
                Fat = Fat + wantedId,
                ProteinPerServing = ProteinPerServing + wantedId,
                Salt = Salt + wantedId,
                Reviews = new HashSet<ReviewViewModel>()
            };

            var amino = await this.service.GetByIdAsync<DetailsAminoViewModel>(wantedId);

            var obj1Str = JsonConvert.SerializeObject(expected);
            var obj2Str = JsonConvert.SerializeObject(amino);

            Assert.NotNull(amino);
            Assert.Equal(obj1Str, obj2Str);
        }

        [Fact]
        public async Task GetAminoByInvalidId_ShouldThrow()
        {
            var startFrom = 1;
            var numberOfAminosToBeSeeded = 1;
            var wantedId = 10;

            this.Initialize();
            this.SeedAminos(startFrom, numberOfAminosToBeSeeded, AminoAcidType.BCAA);

            await Assert.ThrowsAsync<ProductNotFoundException>(() => service
           .GetByIdAsync<DetailsAminoViewModel>(wantedId));
        }

        [Fact]
        public async Task GetAllByValidType_ShouldWorkCorrect()
        {
            var startFrom = 1;
            var numberOfAminosToBeSeeded = 3;
            var wantedType = AminoAcidType.Glutamine.ToString();

            this.Initialize();
            this.SeedAminos(startFrom, numberOfAminosToBeSeeded, AminoAcidType.BCAA);
            this.SeedAminos(startFrom + numberOfAminosToBeSeeded, numberOfAminosToBeSeeded * 2, AminoAcidType.Glutamine);

            var aminos = await this.service.GetAllByTypeAsync<ProductViewModel>(wantedType);

            var expectedCount = 3;
            Assert.Equal(expectedCount, aminos.Count());

            var id = 4;
            Assert.Collection(aminos,
                            item => Assert.Contains(Name + id++, item.Name),
                            item => Assert.Contains(Name + id++, item.Name),
                            item => Assert.Contains(Name + id++, item.Name));
        }

        [Fact]
        public async Task GetAllByValidType_WithNoData_ShouldReturnEmtpy()
        {
            var wantedType = AminoAcidType.Glutamine.ToString();

            this.Initialize();

            var aminos = await this.service.GetAllByTypeAsync<ProductViewModel>(wantedType);

            Assert.Empty(aminos);
        }

        [Fact]
        public async Task GetAllByInvalidType_ShouldThrow()
        {
            var startFrom = 1;
            var numberOfAminosToBeSeeded = 3;
            var wantedTypeInvalid = "InvalidType";

            this.Initialize();
            this.SeedAminos(startFrom, numberOfAminosToBeSeeded, AminoAcidType.BCAA);

            this.Initialize();

            await Assert.ThrowsAsync<InvalidProductTypeException>(() => this.service.GetAllByTypeAsync<ProductViewModel>(wantedTypeInvalid));
        }

        private void SeedAminos(int startFrom, int seedCount, AminoAcidType aminoType)
        {
            var list = new List<Product>();

            for (int i = startFrom; i <= seedCount; i++)
            {
                var amino = new AminoAcid()
                {
                    Id = i,
                    Name = Name + i,
                    Price = Price + i,
                    Description = Description + i,
                    Directions = Directions + i,
                    ImageUrl = ImageUrl + i,
                    ProductType = ProductType.Amino,
                    IsSuatableForVegans = false,
                    Type = aminoType,
                    CreatedOn = DateCreated.AddDays(i),
                    EnergyPerServing = EnergyPerServing + i,
                    Carbohydrate = Carbohydrate + i,
                    Fat = Fat + i,
                    ProteinPerServing = ProteinPerServing + i,
                    Salt = Salt + i
                };

                list.Add(amino);
            }

            this.context.AddRange(list);
            this.context.SaveChanges();
        }

        private void Initialize()
        {
            this.context = KeepFitDbContextInMemoryFactory.Initialize();
            this.mapper = AutoMapperFactory.Initialize();
            this.service = new AminosService(context, mapper);
        }
    }
}