namespace KeepFitStore.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Models.ViewModels.Products.Aminos;
    using KeepFitStore.Services.Contracts;
    
    public class AminosService : IAminosService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public AminosService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<DetailsAminoViewModel> GetByIdAsync(int id)
        {
            var amino = await this.context
                .Aminos
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