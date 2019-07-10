namespace KeepFitStore.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Models.ViewModels.Products.Aminos;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.ViewModels.Products;

    public class AminosService : IAminosService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public AminosService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllByTypeAsync(string type)
        {
            var aminos = await this.context
              .Aminos
              .Where(x => x.Type.ToString() == type)
              .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<ProductViewModel>>(aminos);
            return viewModel;
        }

        public async Task<DetailsAminoViewModel> GetByIdAsync(int id)
        {
            var amino = await this.context
                .Aminos
                .Include(x => x.Reviews)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (amino == null)
            {
                return null;
            }

            var viewModel = this.mapper.Map<DetailsAminoViewModel>(amino);
            return viewModel;
        }
    }
}