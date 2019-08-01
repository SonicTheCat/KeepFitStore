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

    public class CreatinesService : ICreatinesService
    {
        private readonly KeepFitDbContext context;
        private readonly IProductsService productsService;
        private readonly IMapper mapper;

        public CreatinesService(KeepFitDbContext context,IProductsService productsService, IMapper mapper)
        {
            this.context = context;
            this.productsService = productsService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TViewModel>> GetAllByTypeAsync<TViewModel>(string type)
        {
            this.productsService.ValidateProductType(typeof(CreatineType), type);

            var creatines = await this.context
               .Creatines
               .Where(x => x.Type.ToString() == type)
               .AsNoTracking()
               .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(creatines);
            return viewModel;
        }

        public async Task<TViewModel> GetByIdAsync<TViewModel>(int id)
        {
            var creatine = await this.context
                .Creatines
                .Include(x => x.Reviews)
                .ThenInclude(x => x.KeepFitUser)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (creatine == null)
            {
                //TODO: throw service
            }

            var viewModel = this.mapper.Map<TViewModel>(creatine);
            return viewModel;
        }
    }
}