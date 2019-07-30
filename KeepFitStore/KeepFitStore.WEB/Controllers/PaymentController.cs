namespace KeepFitStore.WEB.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using KeepFitStore.Models.InputModels.Payments;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.WEB.Common;

    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService paymentService;
        private readonly IOrdersService ordersService;

        public PaymentController(IPaymentService paymentService, IOrdersService ordersService)
        {
            this.paymentService = paymentService;
            this.ordersService = ordersService;
        }

        [HttpPost]
        public async Task<IActionResult> Charge(ChargePaymentInputModel model)
        {
            var isPaymentCompleted = await this.paymentService.MakePaymentAsync(
                model.StripeEmail, 
                model.StripeToken, 
                model.Amount);

            if (isPaymentCompleted)
            {
                await this.ordersService.CompleteOrderAsync(this.User, model.OrderId); 
            }

            return this.Redirect(WebConstants.OrdersIndexPath); 
        }
    }
}