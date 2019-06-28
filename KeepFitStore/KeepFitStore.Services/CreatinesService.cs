namespace KeepFitStore.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Models.ViewModels.Products.Creatines;
    using KeepFitStore.Services.Contracts;
    
    public class CreatinesService : ICreatinesService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public CreatinesService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<DetailsCreatineViewModel> GetByIdAsync(int id)
        {
            var creatine = await this.context
                .Creatines
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