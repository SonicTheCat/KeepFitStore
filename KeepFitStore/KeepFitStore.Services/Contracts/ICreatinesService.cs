namespace KeepFitStore.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using KeepFitStore.Models.ViewModels.Products;
    using KeepFitStore.Models.ViewModels.Products.Creatines;

    public interface ICreatinesService
    {
        Task<TViewModel> GetByIdAsync<TViewModel>(int id);

        Task<IEnumerable<TViewModel>> GetAllByTypeAsync<TViewModel>(string type);
    }
}