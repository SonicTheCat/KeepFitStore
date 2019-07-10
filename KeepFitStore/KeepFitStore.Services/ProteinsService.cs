namespace KeepFitStore.Services
{
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Models.ViewModels.Products.Proteins;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.ViewModels.Products;

    public class ProteinsService : IProteinsService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public ProteinsService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllByTypeAsync(string type)
        {
            var proteins = await this.context
               .Proteins
               .Where(x => x.Type.ToString() == type)
               .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<ProductViewModel>>(proteins);
            return viewModel; 
        }

        public async Task<DetailsProteinViewModel> GetByIdAsync(int id)
        {
            var protein = await this.context
                .Proteins
                .Include(x => x.Reviews)
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