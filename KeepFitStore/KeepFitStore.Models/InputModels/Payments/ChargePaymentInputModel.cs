namespace KeepFitStore.Models.InputModels.Payments
{
    public class ChargePaymentInputModel
    {
        public string StripeEmail { get; set; }

        public string StripeToken { get; set; }

        public decimal Amount { get; set; }

        public int OrderId { get; set; }
    }
}