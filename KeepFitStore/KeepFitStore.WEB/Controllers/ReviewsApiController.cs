namespace KeepFitStore.WEB.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;

    using KeepFitStore.Models.InputModels.Reviews;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Domain;

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReviewsApiController : ControllerBase
    {
        private readonly IReviewsService reviewsService;
        private readonly UserManager<KeepFitUser> userManager;

        public ReviewsApiController(IReviewsService reviewsService, UserManager<KeepFitUser> userManager)
        {
            this.reviewsService = reviewsService;
            this.userManager = userManager;
        }

        [HttpPost]
        [ActionName(nameof(Create))]
        public async Task Create(CreateReviewInputModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var user = await this.userManager.GetUserAsync(this.User);
                model.KeepFitUserId = user.Id;
            }

            await this.reviewsService.CreateAsync(model);
        }
    }
}