namespace KeepFitStore.Services.Contracts
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IBasketService
    {
        Task AddProductToBasketAsync(int productId, ClaimsPrincipal principal, int? quntity = null); 
    }
}