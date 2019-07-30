namespace KeepFitStore.Domain
{
    using KeepFitStore.Domain.Products;

    public class KeepFitUserFavoriteProducts
    {
        public string KeepFitUserId { get; set; }

        public KeepFitUser KeepFitUser { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}