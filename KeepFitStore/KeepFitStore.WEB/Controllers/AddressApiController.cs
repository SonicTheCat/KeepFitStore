namespace KeepFitStore.WEB.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using KeepFitStore.Models.InputModels.Address;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.ViewModels.Address;
    using Microsoft.AspNetCore.Identity;
    using KeepFitStore.Domain;

    public class AddressApiController : ApiController
    {
        private readonly IAddressService addressService;
        private readonly UserManager<KeepFitUser> userManger;

        public AddressApiController(IAddressService addressService, UserManager<KeepFitUser> userManger)
        {
            this.addressService = addressService;
            this.userManger = userManger;
        }

        [ActionName(nameof(Get))]
        [Authorize]
        public async Task<ActionResult<GetAddressViewModel>> Get()
        {
            var userId = this.userManger.GetUserId(this.User); 
            var obj = await this.addressService.GetAddressFromUser<GetAddressViewModel>(userId);

            return obj;
        }

        [HttpPost]
        [ActionName(nameof(Create))]
        [Authorize]
        public async Task<ActionResult<CreateAddressViewModel>> Create(CreateAddressInputModel model)
        {
            var createdObj = await this.addressService.AddAddressToUserAsync<CreateAddressViewModel>(model, this.User.Identity.Name);

            return createdObj;
        }
    }
}