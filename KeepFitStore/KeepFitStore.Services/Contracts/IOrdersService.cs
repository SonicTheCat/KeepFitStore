namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using KeepFitStore.Models.InputModels.Orders;

    public interface IOrdersService
    {
        Task<CreateOrderInputModel> AddBasketContentToOrderByUserAsync(ClaimsPrincipal principal);

        Task<int> StartCompletingUserOderAsync(ClaimsPrincipal principal, CreateOrderInputModel model);

        Task CompleteOrderAsync(ClaimsPrincipal principal, int orderId);

        Task<TViewModel> GetOrderByIdAsync<TViewModel>(int orderId); 

        Task<IEnumerable<TViewModel>> GetAllOrdersAsync<TViewModel>();

        Task<IEnumerable<TViewModel>> GetAllOrdersForUserAsync<TViewModel>(ClaimsPrincipal principal);

        Task<IEnumerable<TViewModel>> GetAllOrdersForUserSortedAsync<TViewModel>(ClaimsPrincipal principal, string sortBy);

        Task<IEnumerable<TViewModel>> AppendFiltersAndSortOrdersAsync<TViewModel>(string[] filters, string sortBy);

        Task<TViewModel> GetOrderDetailsForUserAsync<TViewModel>(ClaimsPrincipal principal, int orderId);

        Task<TViewModel> GetOrderDetailsAsync<TViewModel>(int orderId);

        Task ChangeOrderCurrentStatusAsync(int orderId, string currentStatus);
    }
}