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

        Task StartCompletingUserOderAsync(ClaimsPrincipal principal, CreateOrderInputModel model);

        Task<IEnumerable<AlllOrdersViewModel>> GetAllOrdersAsync();

        Task<IEnumerable<IndexOrdersViewModel>> GetAllOrdersForUserAsync(ClaimsPrincipal principal); 

        Task<IEnumerable<IndexOrdersViewModel>> GetAllOrdersForUserSortedAsync(ClaimsPrincipal principal, string sortBy); 

        Task<DetailsOrdersViewModel> GetOrderDetailsForUserAsync(ClaimsPrincipal principal, int orderId); 

        Task<DetailsOrdersViewModel> GetOrderDetailsAsync(int orderId); 

        Task ChangeOrderCurrentStatusAsync(int orderId, string currentStatus); 
    }
}