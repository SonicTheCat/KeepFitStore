namespace KeepFitStore.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Data;
    using KeepFitStore.Models.InputModels.JobPositions;
    using KeepFitStore.Domain;

    public class JobPositionService : IJobPositionService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public JobPositionService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<int> CreateAsync(CreateJobPositionInputModel model)
        {
            var doesExist = await this.context
                .Positions
                .AnyAsync(x => x.Name == model.Name && x.Salary == model.Salary);

            if (doesExist)
            {
                return default;
            }

            var position = this.mapper.Map<JobPosition>(model);
            this.context.Positions.Add(position);
            var rowsAdded = await this.context.SaveChangesAsync();

            return rowsAdded; 
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>()
        {
            var positions = await this.context
                .Positions
                .AsNoTracking()
                .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(positions);

            return viewModel;
        }
    }
}