namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using KeepFitStore.Models.ViewModels.Products;
    using KeepFitStore.Models.ViewModels.Products.Proteins;

    public interface IProteinsService
    {
        Task<DetailsProteinViewModel> GetByIdAsync(int id);

        Task<IEnumerable<ProductViewModel>> GetAllByTypeAsync(string type);
    }
}