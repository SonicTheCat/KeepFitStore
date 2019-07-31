namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFavoriteService
    {
        Task<bool> AddAsync(int productId, string username);

        Task<bool> RemoveAsync(int productId, string username);

        Task<IEnumerable<TViewModel>> GetAllByUser<TViewModel>(string username); 
    }
}