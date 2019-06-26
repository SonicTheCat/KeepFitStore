namespace KeepFitStore.Models.ViewModels.Products
{
    using KeepFitStore.Domain.Enums;

    public class NewestProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        // public bool IsOnSale { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public ProductType ProductType { get; set; }
    }
}