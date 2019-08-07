namespace KeepFitStore.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Models.InputModels.User;
    using KeepFitStore.Services.Contracts;
    using Microsoft.EntityFrameworkCore;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.CustomExceptions.Messsages;

    public class UsersSerivce : IUsersService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public UsersSerivce(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TViewModel> UpdateUserOrderInfoAsync<TViewModel>(string id, UpdateUserInputModel model)
        {
            var user = await this.context
                .Users
                .SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new UserNotFoundException(string.Format(ExceptionMessages.UserLookupFailed, id));
            }

            if (user.FullName != model.FullName || user.PhoneNumber != model.PhoneNumber)
            {
                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                await this.context.SaveChangesAsync();
            }

            var viewModel = this.mapper.Map<TViewModel>(user); 
            return viewModel;
        }
    }
}