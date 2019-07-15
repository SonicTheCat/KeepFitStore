using KeepFitStore.Models.InputModels.Orders;
using KeepFitStore.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KeepFitStore.WEB.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var order = await this.ordersService.AddBasketContentToOrderByUserAsync(this.User);

            return this.View(order);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderInputModel inputModel)
        {
            return null;
        }
    }
}