

using KeepFitStore.Models.InputModels.Orders;
using KeepFitStore.Models.ViewModels.Basket;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KeepFitStore.Services.Contracts
{
    public interface IOrdersService
    {
        Task<CreateOrderInputModel> AddBasketContentToOrderByUserAsync(ClaimsPrincipal principal);
    }
}
