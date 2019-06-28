namespace KeepFitStore.Services.Contracts
{
    using System.Threading.Tasks;

    using KeepFitStore.Models.ViewModels.Products.Vitamins;

    public interface IVitaminsService
    {
        Task<DetailsVitaminViewModel> GetByIdAsync(int id);
    }
}