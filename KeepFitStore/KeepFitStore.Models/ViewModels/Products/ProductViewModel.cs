using KeepFitStore.Domain.Enums;

namespace KeepFitStore.Models.ViewModels.Products
{
    public class ProductViewModel
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