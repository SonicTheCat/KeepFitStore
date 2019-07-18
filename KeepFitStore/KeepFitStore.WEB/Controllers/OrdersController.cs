namespace KeepFitStore.WEB.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Models.InputModels.Orders;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Common;
    using KeepFitStore.WEB.Filters;
    using KeepFitStore.Models.ViewModels.Orders;

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

            //TODO: catch service error when implemented! dont compare order to null
            if (order == null)
            {
                return this.Redirect(WebConstants.HomePagePath); 
            }

            return this.View(order);
        }

        [Authorize]
        [HttpPost]
        [ValidateModelStateFilter(nameof(Create))]
        public async Task<IActionResult> Create(CreateOrderInputModel inputModel)
        {
            await this.ordersService.StartCompletingUserOder(this.User, inputModel);

            return this.Redirect(WebConstants.HomePagePath); 
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            var viewModel = await this.ordersService.GetAllOrdersForUserAsync(this.User); 

            return this.View(viewModel); 
        }

        [Authorize]
        public async Task<IActionResult> Details(int orderId)
        {
            var viewModel = await this.ordersService.GetDetailsForOrderAsync(this.User, orderId);

            return this.PartialView("~/Views/Partials/_OrderDetailsPartialView.cshtml", viewModel); 
        }
    }
}