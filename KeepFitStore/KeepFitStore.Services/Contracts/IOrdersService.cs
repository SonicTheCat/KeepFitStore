namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using KeepFitStore.Models.InputModels.Orders;

    public interface IOrdersService
    {
        Task<CreateOrderInputModel> AddBasketContentToOrderByUserAsync(string username);

        Task<int> StartCompletingUserOderAsync(string username, CreateOrderInputModel model);

        Task CompleteOrderAsync(string username, int orderId);

        Task<TViewModel> GetOrderByIdAsync<TViewModel>(int orderId); 

        Task<IEnumerable<TViewModel>> GetAllOrdersAsync<TViewModel>();

        Task<IEnumerable<TViewModel>> GetAllOrdersForUserAsync<TViewModel>(string username);

        Task<IEnumerable<TViewModel>> GetAllOrdersForUserSortedAsync<TViewModel>(string username, string sortBy);

        Task<IEnumerable<TViewModel>> AppendFiltersAndSortOrdersAsync<TViewModel>(string[] filters, string sortBy);

        Task<TViewModel> GetOrderDetailsForUserAsync<TViewModel>(string username, int orderId);

        Task<TViewModel> GetOrderDetailsAsync<TViewModel>(int orderId);

        Task ChangeOrderCurrentStatusAsync(int orderId, string currentStatus);
    }
}