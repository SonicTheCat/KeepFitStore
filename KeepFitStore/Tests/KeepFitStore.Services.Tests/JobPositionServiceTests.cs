namespace KeepFitStore.Services.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using Xunit;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Models.InputModels.JobPositions;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.Tests.Common;
    public class JobPositionServiceTests
    {
        private const string PositionName = "Cashier";
        private const decimal Salary = 1000;

        private const decimal LoopIterations = 10;

        private KeepFitDbContext context;
        private IMapper mapper;
        private IJobPositionService service;

        [Theory]
        [InlineData("Vodoprovodchik", 0.0)]
        [InlineData("Cashier", 3500.0)]
        [InlineData("", 100.9)]
        public async Task CreateNewPosition_ShouldReturnOneRowAdded(string positionName, decimal salary)
        {
            this.Initialize();

            var input = new CreateJobPositionInputModel()
            {
                Name = positionName,
                Salary = salary
            };

            var rowsExpectedToAdd = 1;
            var addedCount = await this.service.CreateAsync(input);

            Assert.Equal(rowsExpectedToAdd, addedCount);
        }

        [Fact]
        public async Task CreatePostionThatAlreadyExist_ShouldReturnDefault()
        {
            this.Initialize();
            this.SeedPositions(); 

            var input1 = new CreateJobPositionInputModel()
            {
                Name = PositionName,
                Salary = Salary
            };

            var input2 = new CreateJobPositionInputModel()
            {
                Name = PositionName + 1,
                Salary = Salary + 1
            };

            var input3 = new CreateJobPositionInputModel()
            {
                Name = PositionName + 9,
                Salary = Salary + 9
            };

            var rowsExpectedToAdd = 0;
            var addedCount1 = await this.service.CreateAsync(input1);
            var addedCount2 = await this.service.CreateAsync(input2);
            var addedCount3 = await this.service.CreateAsync(input3);

            Assert.Equal(rowsExpectedToAdd, addedCount1);
            Assert.Equal(rowsExpectedToAdd, addedCount2);
            Assert.Equal(rowsExpectedToAdd, addedCount3);
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

        private void Initialize()
        {
            this.context = KeepFitDbContextInMemoryFactory.Initialize();
            this.mapper = AutoMapperFactory.Initialize();
            this.service = new JobPositionService(context, mapper);
        }
    }
}