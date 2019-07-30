namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProteinsService
    {
        Task<TViewModel> GetByIdAsync<TViewModel>(int id);

        Task<IEnumerable<TViewModel>> GetAllByTypeAsync<TViewModel>(string type);
    }
}