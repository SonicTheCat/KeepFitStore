namespace KeepFitStore.Services.Contracts
{
    using System.Threading.Tasks;

    using KeepFitStore.Models.InputModels.Jobs;

    public interface IJobApplicantService
    {
        Task<int> AddApplicantAsync(CreateJobApplicantInputModel inputModel);

        Task<TViewModel> GetAllAsync<TViewModel>(); 
    }
}