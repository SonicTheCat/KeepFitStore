namespace KeepFitStore.Services.Contracts
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using KeepFitStore.Models.InputModels.Address;
    using KeepFitStore.Models.ViewModels.Address;

    public interface IAddressService
    {
        Task<CreateAddressViewModel> AddAddressToUserAsync(CreateAddressInputModel model, ClaimsPrincipal principal); 
    }
}