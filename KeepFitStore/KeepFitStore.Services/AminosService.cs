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

    public class AminosService : IAminosService
    {
        private readonly KeepFitDbContext context;
        private readonly IProductsService productsService;
        private readonly IMapper mapper;

        public AminosService(KeepFitDbContext context,IProductsService productsService, IMapper mapper)
        {
            this.context = context;
            this.productsService = productsService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TViewModel>> GetAllByTypeAsync<TViewModel>(string type)
        {
            this.productsService.ValidateProductType(typeof(AminoAcidType), type);

            var aminos = await this.context
              .Aminos
              .Where(x => x.Type.ToString() == type)
              .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(aminos);
            return viewModel;
        }

        public async Task<TViewModel> GetByIdAsync<TViewModel>(int id)
        {
            var amino = await this.context
                .Aminos
                .Include(x => x.Reviews)
                .ThenInclude(x => x.KeepFitUser)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (amino == null)
            {
               //TODO: throw service error 
            }

            var viewModel = this.mapper.Map<TViewModel>(amino);
            return viewModel;
        }
    }
}