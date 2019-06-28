namespace KeepFitStore.Services.Contracts
{
    using System.Threading.Tasks;

    using KeepFitStore.Models.ViewModels.Products.Proteins;

    public interface IProteinsService
    {
        Task<DetailsProteinViewModel> GetByIdAsync(int id);
    }
}