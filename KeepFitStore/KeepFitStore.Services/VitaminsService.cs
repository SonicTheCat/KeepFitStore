namespace KeepFitStore.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Models.ViewModels.Products.Vitamins;
    using KeepFitStore.Services.Contracts;
    
    public class VitaminsService : IVitaminsService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public VitaminsService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<DetailsVitaminViewModel> GetByIdAsync(int id)
        {
            var vitamin = await this.context
                .Vitamins
                .SingleOrDefaultAsync(x => x.Id == id);

            if (vitamin == null)
            {
                return null;
            }

            var viewModel = this.mapper.Map<DetailsVitaminViewModel>(vitamin);
            return viewModel;
        }
    }
}