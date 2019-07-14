namespace KeepFitStore.WEB.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
   
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

        [HttpPost]
        [ActionName(nameof(Create))]
        public async Task<ActionResult<CreateAddressViewModel>> Create(CreateAddressInputModel model)
        {
            var createdObj = await this.addressService.AddAddressToUserAsync(model, this.User);

            return createdObj; 
        }
    }
}