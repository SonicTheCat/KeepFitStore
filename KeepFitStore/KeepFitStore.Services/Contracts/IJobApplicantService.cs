namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using KeepFitStore.Models.InputModels.Jobs;
    
    public interface IJobApplicantService
    {
        Task<int> AddApplicantAsync(CreateJobApplicantInputModel inputModel, IFormFile image);

        Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>(); 
    }
}