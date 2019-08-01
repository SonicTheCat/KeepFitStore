namespace KeepFitStore.WEB.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.InputModels.JobPositions;
    using KeepFitStore.WEB.Filters;
    using KeepFitStore.WEB.Common;

    public class JobPositionController : AdministratorController
    {
        private readonly IJobPositionService positionService;

        public JobPositionController(IJobPositionService positionService)
        {
            this.positionService = positionService;
        }

        public IActionResult Create()   
        {
            return this.View(); 
        }

        [HttpPost]
        [ValidateModelStateFilter(nameof(Create))]
        public async Task<IActionResult> Create(CreateJobPositionInputModel model)
        {
            var rowsCreated = await this.positionService.CreateAsync(model);

            if (rowsCreated != WebConstants.OneRow)
            {
                return this.BadRequest();
            }

            return this.Redirect(WebConstants.AdminHomePage);
        }
    }
}