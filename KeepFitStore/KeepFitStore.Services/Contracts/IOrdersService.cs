namespace KeepFitStore.Services.Contracts
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using KeepFitStore.Models.InputModels.Orders;

    public interface IOrdersService
    {
        Task<CreateOrderInputModel> AddBasketContentToOrderByUserAsync(ClaimsPrincipal principal);

        Task StartCompletingUserOder(ClaimsPrincipal principal, CreateOrderInputModel model);
    }
}