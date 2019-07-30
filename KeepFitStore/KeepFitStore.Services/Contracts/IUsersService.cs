namespace KeepFitStore.Services.Contracts
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using KeepFitStore.Models.InputModels.User;

    public interface IUsersService
    {
        Task<TViewModel> UpdateUserOrderInfoAsync<TViewModel>(
            ClaimsPrincipal claimsPrincipal, 
            UpdateUserInputModel model); 
    }
}