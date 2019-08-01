namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IJobPositionService
    {
        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>(); 
    }
}