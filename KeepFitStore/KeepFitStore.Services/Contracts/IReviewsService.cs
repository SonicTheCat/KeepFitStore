namespace KeepFitStore.Services.Contracts
{
    using System.Threading.Tasks;

    using KeepFitStore.Models.InputModels.Reviews;

    public interface IReviewsService
    {
        Task CreateAsync(CreateReviewInputModel model); 
    }
}