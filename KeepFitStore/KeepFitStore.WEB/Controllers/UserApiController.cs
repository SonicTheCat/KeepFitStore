namespace KeepFitStore.WEB.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    
    using KeepFitStore.Models.InputModels.User;
    using KeepFitStore.Models.ViewModels.User;
    using KeepFitStore.Services.Contracts;
    using Microsoft.AspNetCore.Identity;
    using KeepFitStore.Domain;

    public class UserApiController : ApiController
    {
        private readonly IUsersService usersService;
        private readonly UserManager<KeepFitUser> userManger;

        public UserApiController(IUsersService usersService, UserManager<KeepFitUser> userManger)
        {
            this.usersService = usersService;
            this.userManger = userManger;
        }

        [HttpPost]
        [ActionName(nameof(Update))]
        [Authorize]
        public async Task<ActionResult<UpdateUserViewModel>> Update(UpdateUserInputModel model)
        {
            var userId = this.userManger.GetUserId(this.User);
            var obj = await this.usersService.UpdateUserOrderInfoAsync<UpdateUserViewModel>(userId, model);

            return obj;
        }
    }
}