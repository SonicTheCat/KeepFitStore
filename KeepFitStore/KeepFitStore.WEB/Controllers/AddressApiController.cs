namespace KeepFitStore.WEB.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using KeepFitStore.Models.InputModels.Address;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Models.ViewModels.Address;
    
    public class AddressApiController : ApiController
    {
        private readonly IAddressService addressService;

        public AddressApiController(IAddressService addressService)
        {
            this.addressService = addressService;
        }

        [ActionName(nameof(Get))]
        [Authorize]
        public async Task<ActionResult<GetAddressViewModel>> Get()
        {
            var obj = await this.addressService.GetAddressFromUser(this.User);

            return obj;
        }

        [HttpPost]
        [ActionName(nameof(Create))]
        [Authorize]
        public async Task<ActionResult<CreateAddressViewModel>> Create(CreateAddressInputModel model)
        {
            var createdObj = await this.addressService.AddAddressToUserAsync(model, this.User);

            return createdObj;
        }
    }
}