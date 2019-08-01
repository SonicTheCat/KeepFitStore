namespace KeepFitStore.WEB.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Models.InputModels.Jobs;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.ViewModels.JobPositions;
    using KeepFitStore.WEB.Common;

    public class JobApplicantController : BaseController
    {
        private const int OneRow = 1;
        private const string ViewDataKeyPositions = "positions";

        private readonly IJobApplicantService applicantService;
        private readonly IJobPositionService positionService;

        public JobApplicantController(IJobApplicantService applicantService, IJobPositionService positionService)
        {
            this.applicantService = applicantService;
            this.positionService = positionService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var positions = this.positionService
                .GetAllAsync<JobPositionViewModel>()
                .GetAwaiter()
                .GetResult();

            this.ViewData[ViewDataKeyPositions] = positions; 

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJobApplicantInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(); 
            }

            var rowsCreated = await this.applicantService.AddApplicantAsync(model);

            if (rowsCreated != OneRow)
            {
                return this.BadRequest(); 
            }

            return this.Redirect(WebConstants.HomePagePath); 
        }
    }
}