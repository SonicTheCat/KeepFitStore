namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using KeepFitStore.Models.InputModels.JobPositions;

    public interface IJobPositionService
    {
        Task<int> CreateAsync(CreateJobPositionInputModel model);

        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>(); 
    }
}