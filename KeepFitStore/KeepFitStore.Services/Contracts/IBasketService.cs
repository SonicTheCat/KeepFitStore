namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBasketService
    {
        Task AddProductToBasketAsync(int productId, string username, int? quntity = null);

        Task<IEnumerable<TViewModel>> GetBasketContentAsync<TViewModel>(string username);

        Task<TViewModel> EditBasketItemAsync<TViewModel>(int basketId, int productId, int quantity);

        Task<bool> DeleteBasketItemAsync(int basketId, int productId);

        Task<decimal> GetBasketTotalPriceAsync(string username);

        Task ClearBasketAsync(int basketId); 
    }
}