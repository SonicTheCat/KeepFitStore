namespace KeepFitStore.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Domain.Enums;

    public class VitaminsService : IVitaminsService
    {
        private readonly KeepFitDbContext context;
        private readonly IProductsService productsService;
        private readonly IMapper mapper;

        public VitaminsService(KeepFitDbContext context,IProductsService productsService, IMapper mapper)
        {
            this.context = context;
            this.productsService = productsService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TViewModel>> GetAllByTypeAsync<TViewModel>(string type)
        {
            this.productsService.ValidateProductType(typeof(VitaminType), type); 

            var vitamins = await this.context
              .Vitamins
              .Where(x => x.Type.ToString() == type)
              .AsNoTracking()
              .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(vitamins);
            return viewModel;
        }

        public async Task<TViewModel> GetByIdAsync<TViewModel>(int id)
        {
            var vitamin = await this.context
                .Vitamins
                .Include(x => x.Reviews)
                .ThenInclude(x => x.KeepFitUser)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (vitamin == null)
            {
               //TODO: throw service error; 
            }

            var viewModel = this.mapper.Map<TViewModel>(vitamin);
            return viewModel;
        }
    }
}