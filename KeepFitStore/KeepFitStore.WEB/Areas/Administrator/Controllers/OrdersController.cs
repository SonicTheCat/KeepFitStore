namespace KeepFitStore.WEB.Areas.Administrator.Controllers
{
    using KeepFitStore.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class OrdersController : AdministratorController
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        public async Task<IActionResult> All()
        {
            var allOrders = await this.ordersService.GetAllOrdersAsync();
            return this.View(allOrders); 
        }

        public async Task<IActionResult> Details(int id)
        {
            var orderViewModel = await this.ordersService.GetOrderDetailsAsync(id); 
            return this.View(orderViewModel);
        }

        public async Task<IActionResult> ChangeStatus(int orderId, string currentStatus)
        {
            await this.ordersService.ChangeOrderCurrentStatusAsync(orderId, currentStatus);
            return this.RedirectToAction(nameof(All)); 
        } 
    }
}