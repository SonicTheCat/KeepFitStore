namespace KeepFitStore.Services.Contracts
{
    using System.Threading.Tasks;

    using KeepFitStore.Models.ViewModels.Products.Creatines;

    public interface ICreatinesService
    {
        Task<DetailsCreatineViewModel> GetByIdAsync(int id);
    }
}