﻿namespace KeepFitStore.WEB.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Models.InputModels.Orders;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Common;
    using KeepFitStore.WEB.Filters;
    using KeepFitStore.Models.ViewModels.Orders;
    using KeepFitStore.Domain;
    using Microsoft.AspNetCore.Identity;
    using KeepFitStore.Domain.Enums;

    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;
        private readonly UserManager<KeepFitUser> userManager;

        public OrdersController(IOrdersService ordersService, UserManager<KeepFitUser> userManager)
        {
            this.ordersService = ordersService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var viewModel = await this
                .ordersService
                .GetAllOrdersForUserAsync<IndexOrdersViewModel>(this.User.Identity.Name);

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var order = await this.ordersService.AddBasketContentToOrderByUserAsync(this.User.Identity.Name);

            return this.View(order);
        }

        [Authorize]
        [HttpPost]
        [ValidateModelStateFilter(nameof(Create))]
        public async Task<IActionResult> Create(CreateOrderInputModel inputModel)
        {
            var orderId = await this.ordersService.StartCompletingUserOderAsync(this.User.Identity.Name, inputModel);

            return this.RedirectToAction(nameof(Complete), new { orderId });
        }

        [Authorize]
        public async Task<IActionResult> Complete(int orderId)
        {
            var order = await this.ordersService.GetOrderByIdAsync<CompleteOrderViewModel>(orderId);
            var userId = this.userManager.GetUserId(this.User);

            if (order.KeepFitUserId != userId)
            {
                return this.Forbid();
            }

            if (order.IsCompleted)
            {
                return this.Redirect(WebConstants.HomePagePath);
            }

            if (order.PaymentType != PaymentType.Card)
            {
                await this.ordersService.CompleteOrderAsync(this.User.Identity.Name, order.Id); 
            }

            return this.View(order);
        }

        [Authorize]
        public async Task<IActionResult> AllSorted(string sortBy = WebConstants.DefaultSorting)
        {
            var viewModel = await this
                .ordersService
                .GetAllOrdersForUserSortedAsync<IndexOrdersViewModel>(this.User.Identity.Name, sortBy);

            return this.PartialView("~/Views/Partials/_MyOrdersPartial.cshtml", viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Details(int orderId)
        {
            var viewModel = await this.ordersService
                .GetOrderDetailsForUserAsync<DetailsOrdersViewModel>(this.User.Identity.Name, orderId);

            return this.PartialView("~/Views/Partials/_OrderDetailsPartialView.cshtml", viewModel);
        }
    }
}