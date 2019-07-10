namespace KeepFitStore.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Models.ViewModels.Products.Vitamins;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.ViewModels.Products;

    public class VitaminsService : IVitaminsService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public VitaminsService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllByTypeAsync(string type)
        {
            var vitamins = await this.context
              .Vitamins
              .Where(x => x.Type.ToString() == type)
              .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<ProductViewModel>>(vitamins);
            return viewModel;
        }

        public async Task<DetailsVitaminViewModel> GetByIdAsync(int id)
        {
            var vitamin = await this.context
                .Vitamins
                .Include(x => x.Reviews)
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