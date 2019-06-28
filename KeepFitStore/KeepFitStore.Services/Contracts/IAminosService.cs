namespace KeepFitStore.Services.Contracts
{
    using System.Threading.Tasks;

    using KeepFitStore.Models.ViewModels.Products.Aminos;

    public interface IAminosService
    {
        Task<DetailsAminoViewModel> GetByIdAsync(int id);
    }
}