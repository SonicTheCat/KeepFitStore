namespace KeepFitStore.Services
{
    using System.Threading.Tasks;

    using Stripe;

    using KeepFitStore.Data;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.Common;

    public class PaymentService : IPaymentService
    {
        private readonly KeepFitDbContext context;

        public PaymentService(KeepFitDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> MakePaymentAsync(string email, string token, decimal amount)
        {
            var customersService = new CustomerService();
            var chargesService = new ChargeService();

            var customer = await customersService.CreateAsync(new CustomerCreateOptions()
            {
                Email = email,
                Source = token
            });

            var charge = chargesService.Create(new ChargeCreateOptions()
            {
                Amount = (long)(amount * ServicesConstants.PaymentDefaultStripeCents),
                Description = ServicesConstants.PaymentDescription,
                Currency = ServicesConstants.PaymentDefaultCurrency,
                CustomerId = customer.Id,
                ReceiptEmail = email
            });

            if (charge.Status == ServicesConstants.SucceededPayment)
            {
                //TODO: do something with balanceTransactionId - ex: keep it database
                string balanceTransactionId = charge.BalanceTransactionId;
                return true; 
            }

            return false; 
        }
    }
}