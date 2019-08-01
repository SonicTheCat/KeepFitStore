namespace KeepFitStore.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Http;

    using AutoMapper;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Models.InputModels.Jobs;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.PhotoKeeper;

    public class JobApplicantService : IJobApplicantService
    {
        private readonly KeepFitDbContext context;
        private readonly IMapper mapper;
        private readonly IMyCloudinary cloudinary;

        public JobApplicantService(KeepFitDbContext context, IMapper mapper, IMyCloudinary cloudinary)
        {
            this.context = context;
            this.mapper = mapper;
            this.cloudinary = cloudinary;
        }

        public async Task<int> AddApplicantAsync(CreateJobApplicantInputModel inputModel, IFormFile image)
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
            entity.ImageUrl = this.cloudinary.UploadImage(image);

            this.context.Applicants.Add(entity);

            var rowsAdded = await this.context.SaveChangesAsync();
            return rowsAdded; 
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync<TViewModel>()
        {
            var allApplicants = await this.context
                .Applicants
                .Include(x => x.Position)
                .ToListAsync();

            var viewModel = this.mapper.Map<IEnumerable<TViewModel>>(allApplicants);
            return viewModel; 
        }
    }
}