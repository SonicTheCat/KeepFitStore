namespace KeepFitStore.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Data;

    public class JobPositionService : IJobPositionService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public JobPositionService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>()
        {
            var positions = await this.context.Positions.ToListAsync();
            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(positions);

            return viewModel; 
        }
    }
}