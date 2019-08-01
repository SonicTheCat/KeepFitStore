namespace KeepFitStore.WEB.Areas.Administrator.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Models.ViewModels.JobApplicants;
    using KeepFitStore.Services.Contracts;

    public class JobApplicantController : AdministratorController
    {
        private readonly IJobApplicantService applicantService;

        public JobApplicantController(IJobApplicantService applicantService)
        {
            this.applicantService = applicantService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await this.applicantService.GetAllAsync<IndexJobApplicantViewModel>();

            return this.View(viewModel);
        }
    }
}