namespace KeepFitStore.Services.Tests
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Xunit;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Models.InputModels.User;
    using KeepFitStore.Models.ViewModels.User;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.Tests.Common;
    
    public class UserServiceTests
    {
        private const string UserId = "123Abva113";
        private const string Username = "admin@abv.bg";
        private const string FullName = "admin adminov";
        private const string PhoneNumber = "+3598328342234";

        private const string UserId2 = "aabb10101";
        private const string Username2 = "userTwo@abv.bg";
        private const string FullName2 = "fake adminov";
        private const string PhoneNumber2 = "1111111111";

        private const string TestId = "test123";
        private const string TestUsername = "test@gmail.bg";
        private const string TestFullName = "test testov";
        private const string TestPhone = "0044120302133129321";

        private KeepFitDbContext context;
        private IMapper mapper;
        private IUsersService service;

        [Fact]
        public async Task ChangeUserInfo_ShouldChangeAndReturnViewModel()
        {
            this.Initialize();
            this.SeedUser();

            var inputModel = new UpdateUserInputModel()
            {
                FullName = TestFullName,
                PhoneNumber = TestPhone
            };

            var viewModel = await this.service.UpdateUserOrderInfoAsync<UpdateUserViewModel>(UserId, inputModel);

            Assert.Equal(TestFullName, viewModel.FullName);
            Assert.Equal(TestPhone, viewModel.PhoneNumber);
        }

        [Fact]
        public async Task ChangeUserInfo_ShouldThrow()
        {
            this.Initialize();
            this.SeedUser();

            var inputModel = new UpdateUserInputModel()
            {
                FullName = TestFullName,
                PhoneNumber = TestPhone
            };

            await Assert.ThrowsAsync<UserNotFoundException>(() => this.service.UpdateUserOrderInfoAsync<UpdateUserViewModel>(TestId, inputModel));
        }

        private void SeedUser()
        {
            this.context.Users.Add(new KeepFitUser()
            {
                Id = UserId,
                UserName = Username,
                FullName = FullName,
                PhoneNumber = PhoneNumber
            });

            this.context.Users.Add(new KeepFitUser()
            {
                Id = UserId2,
                UserName = Username2,
                FullName = FullName2,
                PhoneNumber = PhoneNumber2
            });

            this.context.SaveChanges();
        }

        private void Initialize()
        {
            this.context = KeepFitDbContextInMemoryFactory.Initialize();
            this.mapper = AutoMapperFactory.Initialize();
            this.service = new UsersSerivce(context, mapper);
        }
    }
}