namespace KeepFitStore.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;

    using AutoMapper;

    using Moq;

    using Newtonsoft.Json;

    using Xunit;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.Models.InputModels.Products.Aminos;
    using KeepFitStore.Models.InputModels.Products.Creatines;
    using KeepFitStore.Models.InputModels.Products.Proteins;
    using KeepFitStore.Models.InputModels.Products.Vitamins;
    using KeepFitStore.Models.ViewModels.Products;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.PhotoKeeper;
    using KeepFitStore.Services.Tests.Common;
    using Microsoft.EntityFrameworkCore;

    public class ProductsServiceTests
    {
        private const string CreatineName = "CreaPure";
        private const string CreatineDescription = "very good Creatine!";
        private const string CreatineDirections = "Take one spoon a day";
        private const int Price1 = 40;

        private const string Proteinname = "24Isolate";
        private const string ProteinDesciption = "The best isolate on market!";
        private const string ProteinDirections = "na tri chasa po dva pati!";
        private const int Price2 = 30;

        private const string AminoName = "BcaaStronger";
        private const string AminoDescription = "Aminos are very important!";
        private const string AminoDirections = "Two times in a training day";
        private const int Price3 = 100;

        private const string VitaminName = "VitaPlus";
        private const string VitaminDescription = "blqlblqblqlq";
        private const string VitaminDirections = "You can take it every day";
        private const int Price4 = 5;

        private const int Energy = 100;
        private const int Protein = 20;
        private const int Carbo = 1;
        private const int Salt = 2;
        private const int Fat = 1;
        private const int Fibre = 10;

        private const int ProductId = 1000;

        private const int LoopIterations = 1000;
        private const int Days = 100;

        private DateTime CreatedOn = DateTime.Now;

        private const string Url = @"https://upload.wikimedia.org/wikipedia/commons/1/13/Benedict_Cumberbatch_2011.png";

        private KeepFitDbContext context;
        private IMapper mapper;
        private IProductsService service;
        private Mock<IMyCloudinary> myCloudinary;
        private readonly IFormFile image;

        public ProductsServiceTests()
        {
            this.image = this.GetFormFile();
        }

        [Fact]
        public async Task CreateProducts_ShouldCreate()
        {
            this.Initialize();
            
            var protModel = new CreateProteinProductInputModel()
            {
                Name = Proteinname,
                Price = Price2,
                Description = ProteinDesciption,
                Directions = ProteinDirections,
                Image = image,
                Type = ProteinType.Whey,
                IsSuatableForVegans = false,
                EnergyPerServing = Energy,
                ProteinPerServing = Protein,
                Fibre = Fibre,
                Salt = Salt,
                Carbohydrate = Carbo,
                Fat = Fat
            };

            var creaModel = new CreateCreatineProductInputModel()
            {
                Name = CreatineName,
                Price = Price1,
                Description = CreatineDescription,
                Directions = CreatineDirections,
                Image = image,
                Type = CreatineType.Capsules,
                IsSuatableForVegans = false
            };

            var aminoModel = new CreateAminoAcidProducInputModel()
            {
                Name = AminoName,
                Price = Price3,
                Description = AminoDescription,
                Directions = AminoDirections,
                Image = image,
                Type = AminoAcidType.BCAA,
                IsSuatableForVegans = false,
                EnergyPerServing = Energy,
                ProteinPerServing = Protein,
                Carbohydrate = Carbo,
                Salt = Salt,
                Fat = Fat
            };

            var vitaModel = new CreateVitaminProductInputModel()
            {
                Name = VitaminName,
                Price = Price4,
                Description = VitaminDescription,
                Directions = VitaminDirections,
                Image = image,
                Type = VitaminType.Multi,
                IsSuatableForVegans = false
            };

            var expectedResult = 1;
            var actual = await this.service.CreateProductAsync<Vitamin, CreateVitaminProductInputModel>(vitaModel, image);
            Assert.Equal(expectedResult, actual);

            actual = await this.service.CreateProductAsync<AminoAcid, CreateAminoAcidProducInputModel>(aminoModel, image);
            Assert.Equal(expectedResult, actual);

            actual = await this.service.CreateProductAsync<Creatine, CreateCreatineProductInputModel>(creaModel, image);
            Assert.Equal(expectedResult, actual);

            actual = await this.service.CreateProductAsync<Protein, CreateProteinProductInputModel>(protModel, image);
            Assert.Equal(expectedResult, actual);

            var expectedTotalCount = 4;
            Assert.Equal(expectedTotalCount, this.context.Products.Count());
            Assert.Equal(expectedResult, this.context.Proteins.Count());
            Assert.Equal(expectedResult, this.context.Aminos.Count());
            Assert.Equal(expectedResult, this.context.Vitamins.Count());
            Assert.Equal(expectedResult, this.context.Creatines.Count());
        }

        [Fact]
        public async Task GetProductsByCreationDate_ShouldWork()
        {
            this.Initialize();
            this.SeedProducts();

            var wantedCount = 3;
            var products = await this.service.GetNewestProductsAsync<ProductViewModel>(wantedCount);

            var expectedCount = 3;
            Assert.Equal(expectedCount, products.Count());

            var firstExpectedId = ProductId + (LoopIterations - 1);
            var secondExpectedId = ProductId + (LoopIterations - 2);
            var thirdExpectedId = ProductId + (LoopIterations - 3);

            Assert.Collection(products,
                            item => Assert.Equal(firstExpectedId, item.Id),
                            item => Assert.Equal(secondExpectedId, item.Id),
                            item => Assert.Equal(thirdExpectedId, item.Id));
        }

        [Fact]
        public async Task GetProductsByRating_ShouldWork()
        {
            this.Initialize();
            this.SeedProducts();
            this.SeedReviewsToProducts();

            var wantedCount = 5;
            var products = await this.service.GetTopRatedProducts<ProductViewModel>(wantedCount);

            var expectedCount = 5;
            Assert.Equal(expectedCount, products.Count());

            var firstExpectedRating = 5;
            var secondExpectedRating = 5;
            var thirdExpectedRating = 4;
            var fourthExpectedRating = 4;
            var fifthExpectedRating = 3;

            Assert.Collection(products,
                            item => Assert.Equal(firstExpectedRating, item.Rating),
                            item => Assert.Equal(secondExpectedRating, item.Rating),
                            item => Assert.Equal(thirdExpectedRating, item.Rating),
                            item => Assert.Equal(fourthExpectedRating, item.Rating),
                            item => Assert.Equal(fifthExpectedRating, item.Rating));
        }

        [Theory]
        [InlineData(5, 1, "Price", 1000)]
        [InlineData(5, 3, "Price descending", 11)]
        [InlineData(10, 3, "Id descending", 21)]
        [InlineData(10, 4, "Id", 1030)]
        public async Task SortProductsTakePageAndSize_ShouldWork(
            int pageSize,
            int pageNumber,
            string sortBy,
            int index)
        {
            this.Initialize();
            this.SeedProducts();

            var products = await this.service.SearchProductsAsync<ProductViewModel>(pageNumber, pageSize, sortBy);

            var expectedCount = pageSize;
            Assert.Equal(expectedCount, products.Count());

            for (int i = 0; i < pageSize; i++)
            {
                var expectedId = ProductId + Math.Abs(LoopIterations - index++);
                Assert.Equal(expectedId, products[i].Id);
            }
        }

        [Fact]
        public async Task GetAll_ShouldReturnFullCollection()
        {
            this.Initialize();
            this.SeedProducts();

            var products = await this.service.GetAll<ProductViewModel>();

            var expectedCount = 1000;
            Assert.Equal(expectedCount, products.Count());
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyViewModelCollection()
        {
            this.Initialize();

            var products = await this.service.GetAll<ProductViewModel>();

            var expectedCount = 0;
            Assert.Equal(expectedCount, products.Count());
        }

        [Fact]
        public async Task GetProductById_ShouldWork()
        {
            this.Initialize();
            this.SeedProducts();

            var product = await this.service.GetProductByIdAsync<ProductViewModel>(ProductId);

            var expected = new ProductViewModel()
            {
                Id = ProductId,
                Name = Proteinname,
                Price = Price1,
                IsOnSale = false,
                ProductType = ProductType.Protein,
                ImageUrl = "url"
            };

            var expectedToJson = JsonConvert.SerializeObject(expected);
            var actualToJson = JsonConvert.SerializeObject(product);

            Assert.Equal(expectedToJson, actualToJson);
        }

        [Fact]
        public async Task GetProductById_ShouldThrow()
        {
            this.Initialize();
            this.SeedProducts();

            await Assert.ThrowsAsync<ProductNotFoundException>(() => this.service.GetProductByIdAsync<ProductViewModel>(ProductId - 1));
        }

        [Fact]
        public async Task DeleteProduct_ShouldThrow()
        {
            this.Initialize();
            this.SeedProducts();

            await Assert.ThrowsAsync<ProductNotFoundException>(() => this.service.DeleteProductByIdAsync(ProductId - 1));

            await Assert.ThrowsAsync<ProductNotFoundException>(() => this.service.DeleteProductByIdAsync(ProductId + LoopIterations));
        }

        [Fact]
        public async Task DeleteProduct_ShouldDeleteCorrect()
        {
            this.Initialize();
            this.SeedProducts();

            var idToDelete = ProductId + (LoopIterations / 2);
            var deletedCount = await this.service.DeleteProductByIdAsync(idToDelete);

            var expected = 1;
            Assert.Equal(expected, deletedCount);
            deletedCount = await this.service.DeleteProductByIdAsync(ProductId);
            Assert.Equal(expected, deletedCount);

            var newFirst = this.context.Products.OrderBy(x => x.Id).First();
            var expectedIdOnNewFirst = ProductId + 1;
            Assert.Equal(expectedIdOnNewFirst, newFirst.Id);
            Assert.Equal(LoopIterations - 2, this.context.Products.Count());
        }

        [Fact]
        public async Task EditProduct_ShouldEditCorrect()
        {
            this.Initialize();
            this.SeedProducts();

            this.context
                .ChangeTracker
                .Entries()
                .Where((e => e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted))
                .ToList()
                .ForEach(e => e.State = EntityState.Detached);

            var protModel = new EditProteinProductInputModel()
            {
                Id = ProductId,
                Name = Proteinname + "edited",
                Price = Price2,
                Description = ProteinDesciption,
                Directions = ProteinDirections,
                Type = ProteinType.Whey,
                IsSuatableForVegans = false,
                EnergyPerServing = 0,
                ProteinPerServing = 0,
                Fibre = 0,
                Salt = 0,
                Carbohydrate = 0,
                Fat = 0
            };

            //var product = GetProduct(ProductId); 

            //Assert.Equal(Proteinname, product.Name);
            //Assert.Equal("url", product.ImageUrl);
            //Assert.Equal(Price1, product.Price);

            var expectedCount = 1;
            var entitieChanged = await this.service.EditProductAsync<Protein, EditProteinProductInputModel>(protModel, image, ProductId);
            Assert.Equal(expectedCount, entitieChanged);

            //var product = GetProduct(ProductId); 
            //Assert.Equal(Proteinname + "edited", product.Name);
            //Assert.Equal(Url, product.ImageUrl);
            //Assert.Equal(Price2, product.Price);
        }

        [Fact]
        public async Task DeleteWithInvalidId_ShouldThrow()
        {
            this.Initialize();
            this.SeedProducts();

            var protModel = new EditProteinProductInputModel()
            {
                Name = Proteinname + "edited",
                Price = Price2,
                Description = ProteinDesciption,
                Directions = ProteinDirections,
                Type = ProteinType.Whey,
                IsSuatableForVegans = false,
                EnergyPerServing = 0,
                ProteinPerServing = 0,
                Fibre = 0,
                Salt = 0,
                Carbohydrate = 0,
                Fat = 0
            };

            await Assert.ThrowsAsync<ProductNotFoundException>(() =>
            this.service.EditProductAsync<Protein, EditProteinProductInputModel>(protModel, image, ProductId - 1));
        }

        private Product GetProduct(int id)
        {
            return this.context
                .Products
                .AsNoTracking()
                .First(x => x.Id == id);
        }

        private void SeedReviewsToProducts()
        {
            var takeProductsCount = 10;

            var products = this.context
                .Products
                .Take(takeProductsCount)
                .ToList();

            for (int i = 0; i < products.Count / 2; i++)
            {
                products[i].Reviews.Add(new Review()
                {
                    GivenRating = i + 1
                });
            }

            for (int i = products.Count - 1; i >= products.Count / 2; i--)
            {
                products[i].Reviews.Add(new Review()
                {
                    GivenRating = products.Count - i
                });
            }

            this.context.SaveChanges();
        }

        private void SeedProducts()
        {
            var list = new List<Product>();

            for (int i = 0; i < LoopIterations; i++)
            {
                Product product = new Protein()
                {
                    Id = i == 0 ? ProductId : ProductId + i,
                    Name = i == 0 ? Proteinname : Proteinname + i,
                    Price = i == 0 ? Price1 : Price1 + i,
                    Type = ProteinType.Bar,
                    ProductType = ProductType.Protein,
                    CreatedOn = CreatedOn.AddDays(-(Days - i)),
                    ImageUrl = "url"
                };

                list.Add(product);
            }

            this.context.AddRange(list);
            this.context.SaveChanges();
        }

        private IFormFile GetFormFile()
        {
            var content = "Hello from a DummyFake File";
            var fileName = "dummy.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            IFormFile file = new FormFile(ms, 0, 0, content, fileName);

            return file;
        }

        private void Initialize()
        {
            this.context = KeepFitDbContextInMemoryFactory.Initialize();
            this.mapper = AutoMapperFactory.Initialize();
            this.myCloudinary = new Mock<IMyCloudinary>();

            this.myCloudinary.Setup(x => x.UploadImage(this.image))
                             .Returns("url");

            this.service = new ProductsService(context, mapper, myCloudinary.Object);
        }
    }
}