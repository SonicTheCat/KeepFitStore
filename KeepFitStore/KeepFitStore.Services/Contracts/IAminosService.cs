namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using KeepFitStore.Models.ViewModels.Products;
    using KeepFitStore.Models.ViewModels.Products.Aminos;

    public interface IAminosService
    {
        Task<DetailsAminoViewModel> GetByIdAsync(int id);

        Task<IEnumerable<ProductViewModel>> GetAllByTypeAsync(string type);
    }
}