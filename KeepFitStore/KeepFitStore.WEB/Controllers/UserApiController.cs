namespace KeepFitStore.WEB.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    using KeepFitStore.Models.InputModels.User;
    using KeepFitStore.Models.ViewModels.User;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Common;

    public class UserApiController : ApiController
    {
        private readonly IUsersService usersService;

        public UserApiController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpPost]
        [ActionName(nameof(Update))]
        public async Task<ActionResult<UpdateUserViewModel>> Update(UpdateUserInputModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var obj = await this.usersService.UpdateUserOrderInfoAsync(this.User, model);
                return obj; 
            }

            return this.Redirect(WebConstants.HomePagePath);
        }
    }
}