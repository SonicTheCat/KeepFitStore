namespace KeepFitStore.Domain
{
    using KeepFitStore.Domain.Products;

    public class Review
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int GivenRating { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string KeepFitUserId { get; set; }

        public KeepFitUser KeepFitUser { get; set; }
    }
}