namespace KeepFitStore.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Models.InputModels.User;
    using KeepFitStore.Models.ViewModels.User;
    using KeepFitStore.Services.Contracts;
    
    public class UsersSerivce : IUsersService
    {
        private readonly UserManager<KeepFitUser> userManager;
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public UsersSerivce(UserManager<KeepFitUser> userManager, KeepFitDbContext context, IMapper mapper)
        {
            this.userManager = userManager;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<UpdateUserViewModel> UpdateUserOrderInfoAsync(ClaimsPrincipal principal, UpdateUserInputModel model)
        {
            var user = await this.userManager.GetUserAsync(principal);
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            await this.context.SaveChangesAsync();

            var viewModel = this.mapper.Map<UpdateUserViewModel>(user); 
            return viewModel;
        }
    }
}