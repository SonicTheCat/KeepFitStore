namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using KeepFitStore.Models.InputModels.Orders;
    using KeepFitStore.Models.ViewModels.Orders;

    public interface IOrdersService
    {
        Task<CreateOrderInputModel> AddBasketContentToOrderByUserAsync(ClaimsPrincipal principal);

        Task StartCompletingUserOder(ClaimsPrincipal principal, CreateOrderInputModel model);

        Task<IEnumerable<AllOrdersViewModel>> GetAllOrdersForUserAsync(ClaimsPrincipal principal); 

        Task<IEnumerable<AllOrdersViewModel>> GetAllOrdersForUserSortedAsync(ClaimsPrincipal principal, string sortBy); 

        Task<DetailsOrdersViewModel> GetDetailsForOrderAsync(ClaimsPrincipal principal, int orderId); 
    }
}