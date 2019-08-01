namespace KeepFitStore.Services
{
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Domain.Enums;

    public class ProteinsService : IProteinsService
    {
        private readonly KeepFitDbContext context;
        private readonly IProductsService productsService;
        private readonly IMapper mapper;

        public ProteinsService(KeepFitDbContext context, IProductsService productsService, IMapper mapper)
        {
            this.context = context;
            this.productsService = productsService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TViewModel>> GetAllByTypeAsync<TViewModel>(string type)
        {
            this.productsService.ValidateProductType(typeof(ProteinType), type);

            var proteins = await this.context
               .Proteins
               .Where(x => x.Type.ToString() == type)
               .AsNoTracking()
               .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(proteins);
            return viewModel;
        }

        public async Task<TViewModel> GetByIdAsync<TViewModel>(int id)
        {
            var protein = await this.context
                .Proteins
                .Include(x => x.Reviews)
                .ThenInclude(x => x.KeepFitUser)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (protein == null)
            {
                //TODO: throw service error
            }

            var viewModel = this.mapper.Map<TViewModel>(protein);
            return viewModel;
        }
    }
}