namespace KeepFitStore.Models
{
    using Products;

    public class BasketItem
    {
        public int BasketId { get; set; }
        public Basket Basket { get; set; }

        public int ProductId { get; set; }
        public Product Products { get; set; }

        public int Quantity { get; set; }
    }
}