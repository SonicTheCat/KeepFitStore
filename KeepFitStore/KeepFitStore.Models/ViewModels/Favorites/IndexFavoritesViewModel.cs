namespace KeepFitStore.Models.ViewModels.Favorites
{
    using KeepFitStore.Domain.Enums;

    public class IndexFavoritesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        //public bool IsOnSale { get; set; }

        public string ImageUrl { get; set; }

        public ProductType ProductType { get; set; }

        public int Rating { get; set; }
    }
}