namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using KeepFitStore.Models.ViewModels.Basket;

    public interface IBasketService
    {
        Task AddProductToBasketAsync(int productId, ClaimsPrincipal principal, int? quntity = null);

        Task<IEnumerable<IndexBasketViewModel>> GetBasketContentAsync(ClaimsPrincipal principal);

        Task<object> EditBasketItemAsync(int basketId, int productId, int quantity); 
    }
}