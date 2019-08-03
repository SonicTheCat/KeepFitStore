namespace KeepFitStore.Services.Contracts
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using KeepFitStore.Models.InputModels.Address;

    public interface IAddressService
    {
        Task<TViewModel> AddAddressToUserAsync<TViewModel>(CreateAddressInputModel model, ClaimsPrincipal principal);

        Task<TViewModel> GetAddressFromUser<TViewModel>(string id);
    }
}