namespace KeepFitStore.Models.ViewModels.Orders
{
    using KeepFitStore.Domain.Enums;

    public class DetailsOrdersProductsViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductImageUrl { get; set; }

        public ProductType ProductProductType { get; set; }

        public int ProductQuantity { get; set; }
    }
}