namespace KeepFitStore.WEB.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Models.InputModels.Address;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.ViewModels.Address;
    using KeepFitStore.WEB.Common;

    public class AddressApiController : ApiController
    {
        private readonly IAddressService addressService;

        public AddressApiController(IAddressService addressService)
        {
            this.addressService = addressService;
        }

        [ActionName(nameof(Get))]
        public async Task<ActionResult<GetAddressViewModel>> Get()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var obj = await this.addressService.GetAddressFromUser(this.User);
                return obj;
            }

            return this.Redirect(WebConstants.HomePagePath);
        }

        [HttpPost]
        [ActionName(nameof(Create))]
        public async Task<ActionResult<CreateAddressViewModel>> Create(CreateAddressInputModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var createdObj = await this.addressService.AddAddressToUserAsync(model, this.User);
                return createdObj;
            }

            return this.Redirect(WebConstants.HomePagePath);
        }
    }
}