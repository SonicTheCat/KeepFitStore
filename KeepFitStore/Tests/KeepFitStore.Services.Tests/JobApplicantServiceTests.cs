namespace KeepFitStore.Services.Tests
{
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
    using KeepFitStore.Models.InputModels.Jobs;
    using KeepFitStore.Models.ViewModels.JobApplicants;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.PhotoKeeper;
    using KeepFitStore.Services.Tests.Common;

    public class JobApplicantServiceTests
    {
        private const int Id = 50;
        private const string FirstName = "Julia";
        private const string MiddleName = "Ivanova";
        private const string LastName = "Draganova";
        private const string PhoneNumber = "+39392932923";
        private const int Age = 20;
        private const string Bio = "My personal biography!";
        private const string Url = @"https://upload.wikimedia.org/wikipedia/commons/1/13/Benedict_Cumberbatch_2011.png";

        private const int PositionId = 1001;
        private const string PositionName = "Cashier";
        private const string PositionTwoName = "Manager";
        private const decimal Salary = 1000;

        private const int LoopIterations = 10;

        private KeepFitDbContext context;
        private IMapper mapper;
        private IJobApplicantService service;
        private Mock<IMyCloudinary> myCloudinary;
        // private Mock<IFormFile> formFile;

        [Fact]
        public async Task CreateApplicant_ShouldReturnCorrect()
        {
            this.Initialize();
            this.SeedPositions();

            var image = this.GetFormFile();
            var input = new CreateJobApplicantInputModel()
            {
                Firstname = FirstName,
                Middlename = MiddleName,
                Lastname = LastName,
                PhoneNumber = PhoneNumber,
                Bio = Bio,
                Age = Age,
                ExperienceInSelling = false,
                Position = PositionName,
            };

            var input2 = new CreateJobApplicantInputModel()
            {
                Firstname = FirstName,
                Middlename = MiddleName,
                Lastname = LastName,
                PhoneNumber = PhoneNumber,
                Bio = Bio,
                Age = Age,
                ExperienceInSelling = true,
                Position = PositionName + 1,
            };

            var expectedCount = 1;
            var addedCount = await this.service.AddApplicantAsync(input, image);
            Assert.Equal(expectedCount, addedCount);
            Assert.Equal(expectedCount, this.context.Applicants.Count());

            addedCount = await this.service.AddApplicantAsync(input2, image);
            Assert.Equal(expectedCount, addedCount);
            expectedCount = 2;
            Assert.Equal(expectedCount, this.context.Applicants.Count());
        }

        [Fact]
        public async Task CreateApplicantInvalidPosition_ShouldThrowJobExc()
        {
            this.Initialize();
            this.SeedPositions();

            var image = this.GetFormFile();
            var input = new CreateJobApplicantInputModel()
            {
                Firstname = FirstName,
                Middlename = MiddleName,
                Lastname = LastName,
                PhoneNumber = PhoneNumber,
                Bio = Bio,
                Age = Age,
                ExperienceInSelling = false,
                Position = PositionTwoName,
            };

            await Assert.ThrowsAsync<JobPositionNotFoundException>(() => this.service.AddApplicantAsync(input, image));
        }

        [Fact]
        public async Task GetAll_ShouldReturnFullCollection()
        {
            this.Initialize();
            this.SeedApplicants();

            var expectedFirst = new IndexJobApplicantViewModel()
            {
                Id = Id,
                Firstname = FirstName,
                Middlename = MiddleName,
                Lastname = LastName,
                PhoneNumber = PhoneNumber,
                Bio = Bio,
                Age = Age,
                ExperienceInSelling = false,
                ImageUrl = Url,
                PositionName = PositionName,
                PositionId = PositionId
            };

            var numToAdd = LoopIterations - 1;
            var expectedLast = new IndexJobApplicantViewModel()
            {
                Id = Id + numToAdd,
                Firstname = FirstName + numToAdd,
                Middlename = MiddleName,
                Lastname = LastName,
                PhoneNumber = PhoneNumber,
                Bio = Bio,
                Age = Age + numToAdd,
                ExperienceInSelling = false,
                ImageUrl = Url,
                PositionName = PositionName + numToAdd,
                PositionId = PositionId + numToAdd
            };

            var viewModel = await this.service.GetAllAsync<IndexJobApplicantViewModel>();

            Assert.NotEmpty(viewModel);

            var expectedCount = 10;

            var actualFirst = viewModel.OrderBy(x => x.Id).First();
            var actualLast = viewModel.OrderByDescending(x => x.Id).First();

            var expectedFirstToJson = JsonConvert.SerializeObject(expectedFirst);
            var actualFirstToJson = JsonConvert.SerializeObject(actualFirst);

            var expectedLastToJson = JsonConvert.SerializeObject(expectedLast);
            var actualLastToJson = JsonConvert.SerializeObject(actualLast);

            Assert.Equal(expectedCount, viewModel.Count());
            Assert.Equal(expectedFirstToJson, actualFirstToJson);
            Assert.Equal(expectedLastToJson, actualLastToJson);
        }

        [Fact]
        public async Task GetAll_ShouldReturnEmptyCollection()
        {
            this.Initialize();

            var viewModel = await this.service.GetAllAsync<IndexJobApplicantViewModel>();
            Assert.Empty(viewModel);
        }

        private void SeedApplicants()
        {
            var list = new List<JobApplicant>();

            for (int i = 0; i < LoopIterations; i++)
            {
                var input = new JobApplicant()
                {
                    Id = i == 0 ? Id : Id + i,
                    Firstname = i == 0 ? FirstName : FirstName + i,
                    Middlename = MiddleName,
                    Lastname = LastName,
                    PhoneNumber = PhoneNumber,
                    Bio = Bio,
                    Age = i == 0 ? Age : Age + i,
                    ExperienceInSelling = false,
                    ImageUrl = Url,
                    Position = new JobPosition()
                    {
                        Id = i == 0 ? PositionId : PositionId + i,
                        Name = i == 0 ? PositionName : PositionName + i,
                        Salary = Salary
                    }
                };

                list.Add(input);
            }

            this.context.Applicants.AddRange(list);
            this.context.SaveChanges();
        }

        private void SeedPositions()
        {
            var list = new List<JobPosition>();

            for (int i = 0; i < LoopIterations; i++)
            {
                var position = new JobPosition
                {
                    Name = i == 0 ? PositionName : PositionName + i,
                    Salary = i == 0 ? Salary : Salary + i
                };

                list.Add(position);
            }

            this.context.Positions.AddRange(list);
            this.context.SaveChanges();
        }

        private IFormFile GetFormFile()
        {
            var str = "This is a dummy file";
            var name = "data";
            var fileName = "dummy.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(str));
            IFormFile file = new FormFile(stream, 0, 0, name, fileName);

            return file;
        }

        private void Initialize()
        {
            this.context = KeepFitDbContextInMemoryFactory.Initialize();
            this.mapper = AutoMapperFactory.Initialize();
            this.myCloudinary = new Mock<IMyCloudinary>();
            //this.formFile = new Mock<IFormFile>();

            this.myCloudinary.Setup(x => x.UploadImage(this.GetFormFile()))
                             .Returns(Url);

            this.service = new JobApplicantService(context, mapper, myCloudinary.Object);
        }
    }
}