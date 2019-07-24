namespace KeepFitStore.WEB.Areas.Administrator.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Common;
    
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

        public async Task<IActionResult> Filter(
            string[] filters, 
            string sortBy = WebConstants.DefaultSortingById)
        {
            var viewModel = await this.ordersService.AppendFiltersAndSortOrdersAsync(filters, sortBy); 
            return this.PartialView("~/Areas/Administrator/Views/Partials/_AdminListAllOrdersPartial.cshtml", viewModel);
        }
    }
}