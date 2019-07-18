namespace KeepFitStore.WEB.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using KeepFitStore.Models.ViewModels.Orders;
    using KeepFitStore.Services.Contracts;
    
    public class OrdersApiController : ApiController
    {
        private readonly IOrdersService ordersService;

        public OrdersApiController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        [HttpGet]
        [ActionName(nameof(Details))]
       // [Route("api/[controller]/[action]/{id}")]
        [Authorize]
        public async Task<ActionResult<DetailsOrdersViewModel>> Details([FromQuery] int orderId)
        {
            var obj = await this.ordersService.GetDetailsForOrderAsync(this.User, orderId);

            return obj; 
        }
    }
}