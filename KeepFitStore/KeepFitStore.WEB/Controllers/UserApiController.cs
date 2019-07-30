namespace KeepFitStore.WEB.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    
    using KeepFitStore.Models.InputModels.User;
    using KeepFitStore.Models.ViewModels.User;
    using KeepFitStore.Services.Contracts;
    
    public class UserApiController : ApiController
    {
        private readonly IUsersService usersService;

        public UserApiController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpPost]
        [ActionName(nameof(Update))]
        [Authorize]
        public async Task<ActionResult<UpdateUserViewModel>> Update(UpdateUserInputModel model)
        {
            var obj = await this.usersService.UpdateUserOrderInfoAsync<UpdateUserViewModel>(this.User, model);

            return obj;
        }
    }
}