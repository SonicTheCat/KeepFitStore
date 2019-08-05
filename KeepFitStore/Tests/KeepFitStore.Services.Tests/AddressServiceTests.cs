namespace KeepFitStore.Services.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    
    using Xunit;
    
    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Models.InputModels.Address;
    using KeepFitStore.Models.ViewModels.Address;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.Tests.Common;

    public class AddressServiceTests
    {
        //var userStore = new Mock<IUserStore<KeepFitUser>>();

        //userStore.Setup(s => s.FindByIdAsync(userId, CancellationToken.None)).ReturnsAsync(context.Users.First(x => x.Id == userId));

        private const string UserOneId = "123";
        private const string UserTwoId = "axxjajxa";
        private const string UserOneName = "admin@abv.bg";
        private const string UserTwoName = "unknown@abv.bg";

        private const string CityOneName = "Sofia";
        private const string CityTwoName = "Plovdiv";

        private const string PostcodeFirst = "1003";
        private const string PostcodeSecond = "CV1 1PQ";

        private const string StreetNameFirst = "Vitosha";
        private const string StreetNameSecond = "Bitolq";

        private const int NumberStrFirst = 10;
        private const int NumberStrSecond = 2001;

        private KeepFitDbContext context;
        private IMapper mapper;
        private IAddressService service;

        [Fact]
        public async Task AddNonExistingAddressToUserWhoHasNoAddress_ShouldAdd()
        {
            this.Initialize();
            this.SeedUser();

            var input = new CreateAddressInputModel()
            {
                CityName = CityOneName,
                PostCode = PostcodeFirst,
                StreetName = StreetNameFirst,
                StreetNumber = NumberStrFirst
            };

            var viewModel = await service.AddAddressToUserAsync<CreateAddressViewModel>(input, UserOneName);

            var expectedCount = 1;
            Assert.Equal(expectedCount, context.Addresses.Count());

            Assert.Equal(CityOneName, viewModel.CityName);
            Assert.Equal(PostcodeFirst, viewModel.CityPostCode);
            Assert.Equal(StreetNameFirst, viewModel.StreetName);
            Assert.Equal(NumberStrFirst, viewModel.StreetNumber);
        }

        [Fact]
        public async Task AddExistingAddressToUserWhoHasNoAddress_ShouldAdd()
        {
            this.Initialize();
            this.SeedUser();
            this.SeedAddress();

            var input = new CreateAddressInputModel()
            {
                CityName = CityOneName,
                PostCode = PostcodeFirst,
                StreetName = StreetNameFirst,
                StreetNumber = NumberStrFirst
            };

            var viewModel = await service.AddAddressToUserAsync<CreateAddressViewModel>(input, UserOneName);

            var expectedCount = 1;
            Assert.Equal(expectedCount, context.Addresses.Count());

            Assert.Equal(CityOneName, viewModel.CityName);
            Assert.Equal(PostcodeFirst, viewModel.CityPostCode);
            Assert.Equal(StreetNameFirst, viewModel.StreetName);
            Assert.Equal(NumberStrFirst, viewModel.StreetNumber);
        }

        [Fact]
        public async Task AddNewAddressToUser_ShouldAdd()
        {
            this.Initialize();
            this.SeedUser();
            this.SeedUserToAddress();

            var input = new CreateAddressInputModel()
            {
                CityName = CityTwoName,
                PostCode = PostcodeFirst,
                StreetName = StreetNameFirst,
                StreetNumber = NumberStrFirst
            };

            var viewModel = await service.AddAddressToUserAsync<CreateAddressViewModel>(input, UserOneName);

            var expectedCount = 2;
            Assert.Equal(expectedCount, context.Addresses.Count());

            Assert.Equal(CityTwoName, viewModel.CityName);
            Assert.Equal(PostcodeFirst, viewModel.CityPostCode);
            Assert.Equal(StreetNameFirst, viewModel.StreetName);
            Assert.Equal(NumberStrFirst, viewModel.StreetNumber);
        }

        [Fact]
        public async Task AddSameAddress_ShouldNotAdd()
        {
            this.Initialize();
            this.SeedUser();
            this.SeedUserToAddress();

            var input = new CreateAddressInputModel()
            {
                CityName = CityOneName,
                PostCode = PostcodeFirst,
                StreetName = StreetNameFirst,
                StreetNumber = NumberStrFirst
            };

            var viewModel = await service.AddAddressToUserAsync<CreateAddressViewModel>(input, UserOneName);

            var expectedCount = 1;
            Assert.Equal(expectedCount, context.Addresses.Count());
            Assert.Equal(CityOneName, viewModel.CityName);
        }

        [Fact]
        public async Task AddAddressWithDiffrentPostCode_ShouldAdd()
        {
            this.Initialize();
            this.SeedUser();
            this.SeedAddress();

            var input = new CreateAddressInputModel()
            {
                CityName = CityOneName,
                PostCode = PostcodeSecond,
                StreetName = StreetNameFirst,
                StreetNumber = NumberStrFirst
            };

            var viewModel = await service.AddAddressToUserAsync<CreateAddressViewModel>(input, UserOneName);

            var expectedCount = 2;
            Assert.Equal(expectedCount, context.Addresses.Count());
            Assert.Equal(CityOneName, viewModel.CityName);
            Assert.Equal(PostcodeSecond, viewModel.CityPostCode);
        }

        [Fact]
        public async Task AddAddressWithDiffrentStreetName_ShouldAdd()
        {
            this.Initialize();
            this.SeedUser();
            this.SeedAddress();

            var input = new CreateAddressInputModel()
            {
                CityName = CityOneName,
                PostCode = PostcodeFirst,
                StreetName = StreetNameSecond,
                StreetNumber = NumberStrFirst
            };

            var viewModel = await service.AddAddressToUserAsync<CreateAddressViewModel>(input, UserOneName);

            var expectedCount = 2;
            Assert.Equal(expectedCount, context.Addresses.Count());
            Assert.Equal(CityOneName, viewModel.CityName);
            Assert.Equal(StreetNameSecond, viewModel.StreetName);
        }

        [Fact]
        public async Task AddAddressWithDiffrentStreetNumber_ShouldAdd()
        {
            this.Initialize();
            this.SeedUser();
            this.SeedAddress();

            var input = new CreateAddressInputModel()
            {
                CityName = CityOneName,
                PostCode = PostcodeFirst,
                StreetName = StreetNameFirst,
                StreetNumber = NumberStrSecond
            };

            var viewModel = await service.AddAddressToUserAsync<CreateAddressViewModel>(input, UserOneName);

            var expectedCount = 2;
            Assert.Equal(expectedCount, context.Addresses.Count());
            Assert.Equal(CityOneName, viewModel.CityName);
            Assert.Equal(NumberStrSecond, viewModel.StreetNumber);
        }

        [Fact]
        public async Task AddAddressToFakeUser_ShouldThrow()
        {
            this.Initialize();
            this.SeedUser();

            var input = new CreateAddressInputModel()
            {
                CityName = CityOneName,
                PostCode = PostcodeFirst,
                StreetName = StreetNameFirst,
                StreetNumber = NumberStrFirst
            };

            await Assert.ThrowsAsync<NullReferenceException>(() => service
            .AddAddressToUserAsync<CreateAddressViewModel>(input, UserTwoName));
        }

        [Fact]
        public async Task GetExistingAddressByUserId_ShouldReturnViewModel()
        {
            this.Initialize();
            this.SeedUser();
            this.SeedUserToAddress();

            var viewModel = await this.service
                .GetAddressFromUser<GetAddressViewModel>(UserOneId);

            Assert.Equal(CityOneName, viewModel.CityName);
            Assert.Equal(PostcodeFirst, viewModel.CityPostCode);
            Assert.Equal(NumberStrFirst, viewModel.StreetNumber);
            Assert.Equal(StreetNameFirst, viewModel.StreetName);
        }

        [Fact]
        public async Task GetAddressFromNotExistingUser_ShouldThrow()
        {
            this.Initialize();
            
            await Assert.ThrowsAsync<UserNotFoundException>(() => service
            .GetAddressFromUser<GetAddressViewModel>(UserTwoId));
        }

        private void SeedUserToAddress()
        {
            var user = this.context
                .Users
                .Single(x => x.UserName == UserOneName);

            user.Address = new Address()
            {
                City = new City()
                {
                    Name = CityOneName,
                    PostCode = PostcodeFirst
                },
                StreetNumber = NumberStrFirst,
                StreetName = StreetNameFirst
            };

            this.context.SaveChanges();
        }

        private void SeedUser()
        {
            var user = new KeepFitUser()
            {
                Id = UserOneId,
                UserName = UserOneName,
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        private void SeedAddress()
        {
            var address = new Address()
            {
                City = new City()
                {
                    Name = CityOneName,
                    PostCode = PostcodeFirst
                },
                StreetNumber = NumberStrFirst,
                StreetName = StreetNameFirst
            };

            this.context.Addresses.Add(address);
            this.context.SaveChanges();
        }

        private void Initialize()
        {
            this.context = KeepFitDbContextInMemoryFactory.Initialize();
            this.mapper = AutoMapperFactory.Initialize();
            this.service = new AddressService(context, mapper);
        }
    }
}