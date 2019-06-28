namespace KeepFitStore.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Models.ViewModels.Products.Proteins;
    using KeepFitStore.Services.Contracts;
    
    public class ProteinsService : IProteinsService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public ProteinsService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<DetailsProteinViewModel> GetByIdAsync(int id)
        {
            var protein = await this.context
                .Proteins
                .SingleOrDefaultAsync(x => x.Id == id);

            if (protein == null)
            {
                return null;
            }

            var viewModel = this.mapper.Map<DetailsProteinViewModel>(protein);
            return viewModel;
        }
    }
}