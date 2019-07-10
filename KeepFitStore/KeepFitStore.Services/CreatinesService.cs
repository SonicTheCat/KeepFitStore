namespace KeepFitStore.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Models.ViewModels.Products.Creatines;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.ViewModels.Products;

    public class CreatinesService : ICreatinesService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public CreatinesService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllByTypeAsync(string type)
        {
            var creatines = await this.context
               .Creatines
               .Where(x => x.Type.ToString() == type)
               .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<ProductViewModel>>(creatines);
            return viewModel;
        }

        public async Task<DetailsCreatineViewModel> GetByIdAsync(int id)
        {
            var creatine = await this.context
                .Creatines
                .Include(x => x.Reviews)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (creatine == null)
            {
                return null;
            }

            var viewModel = this.mapper.Map<DetailsCreatineViewModel>(creatine);
            return viewModel;
        }
    }
}