namespace KeepFitStore.Services
{
    using System.Threading.Tasks;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Models.InputModels.Jobs;
    using KeepFitStore.Services.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class JobApplicantService : IJobApplicantService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;

        public JobApplicantService(KeepFitDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<int> AddApplicantAsync(CreateJobApplicantInputModel inputModel)
        {
            var jobPosition = await this.context
                .Positions
                .SingleOrDefaultAsync(x => x.Name == inputModel.Position);

            if (jobPosition == null)
            {
                //TODO: throw service error; 
            }

            var entity = this.mapper.Map<JobApplicant>(inputModel);
            entity.Position = jobPosition; 

            this.context.Applicants.Add(entity);

            var rowsAdded = await this.context.SaveChangesAsync();
            return rowsAdded; 
        }

        public async Task<TViewModel> GetAllAsync<TViewModel>()
        {
            var allApplicants = await this.context
                .Applicants
                .Include(x => x.Position)
                .ToListAsync();

            var viewModel = this.mapper.Map<TViewModel>(allApplicants);
            return viewModel; 
        }
    }
}