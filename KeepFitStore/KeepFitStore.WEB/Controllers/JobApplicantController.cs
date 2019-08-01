namespace KeepFitStore.WEB.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Models.InputModels.Jobs;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.ViewModels.JobPositions;
    using KeepFitStore.WEB.Filters;
 
    public class JobApplicantController : BaseController
    {
        private readonly IJobPositionService positionService;

        public JobApplicantController(IJobPositionService positionService)
        {
            this.positionService = positionService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var positions = this.positionService
                .GetAllAsync<JobPositionViewModel>()
                .GetAwaiter()
                .GetResult();

            this.ViewData["positions"] = positions; 

            return this.View();
        }

        [HttpPost]
       
        public IActionResult CreateProcces(CreateJobsInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                ;
            }
            return this.RedirectToAction("/Home/Index"); 
        }
    }
}