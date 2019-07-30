namespace KeepFitStore.Models.ViewModels.Orders
{
    public class CompleteOrderProductsViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductImageUrl { get; set; }

        public int Quantity { get; set; }
    }
}
