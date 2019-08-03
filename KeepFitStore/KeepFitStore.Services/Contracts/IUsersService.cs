namespace KeepFitStore.Services.Contracts
{
    using System.Threading.Tasks;

    using KeepFitStore.Models.InputModels.User;

    public interface IUsersService
    {
        Task<TViewModel> UpdateUserOrderInfoAsync<TViewModel>(
            string id, 
            UpdateUserInputModel model); 
    }
}