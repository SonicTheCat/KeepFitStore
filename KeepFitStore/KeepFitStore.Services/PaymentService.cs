namespace KeepFitStore.Services
{
    using System.Threading.Tasks;

    using Stripe;

    using KeepFitStore.Data;
    using KeepFitStore.Services.Contracts;
    
    public class PaymentService : IPaymentService
    {
        private const string DefaultCurrency = "gbp";
        private const string PaymentDescription = "Paying for supliments in keep fit store";
        private const string SucceededPayment = "succeeded";
        private const int DefaultStripeCents = 100;

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
                Amount = (long)(amount * DefaultStripeCents),
                Description = PaymentDescription,
                Currency = DefaultCurrency,
                CustomerId = customer.Id,
                ReceiptEmail = email
            });

            if (charge.Status == SucceededPayment)
            {
                //TODO: do something with balanceTransactionId
                string balanceTransactionId = charge.BalanceTransactionId;
                return true; 
            }

            return false; 
        }
    }
}