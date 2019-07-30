namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IBasketService
    {
        Task AddProductToBasketAsync(int productId, ClaimsPrincipal principal, int? quntity = null);

        Task<IEnumerable<TViewModel>> GetBasketContentAsync<TViewModel>(ClaimsPrincipal principal);

        Task<TViewModel> EditBasketItemAsync<TViewModel>(int basketId, int productId, int quantity);

        Task<bool> DeleteBasketItemAsync(int basketId, int productId);

        Task<decimal> GetBasketTotalPriceAsync(ClaimsPrincipal principal);

        Task ClearBasketAsync(int basketId); 
    }
}