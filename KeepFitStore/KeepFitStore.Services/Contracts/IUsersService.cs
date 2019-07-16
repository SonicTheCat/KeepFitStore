namespace KeepFitStore.Services.Contracts
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using KeepFitStore.Models.InputModels.User;
    using KeepFitStore.Models.ViewModels.User;

    public interface IUsersService
    {
        Task<UpdateUserViewModel> UpdateUserOrderInfoAsync(ClaimsPrincipal claimsPrincipal, UpdateUserInputModel model); 
    }
}