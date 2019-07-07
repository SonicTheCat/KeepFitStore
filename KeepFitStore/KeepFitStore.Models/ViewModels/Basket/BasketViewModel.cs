namespace KeepFitStore.Models.ViewModels.Basket
{
    using KeepFitStore.Models.ViewModels.Products;

    public class BasketViewModel
    {
        public int BasketId { get; set; }

        public int Quantity { get; set; }

        public ProductInBasketViewModel Product { get; set; }
    }
}