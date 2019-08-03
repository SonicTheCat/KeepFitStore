namespace KeepFitStore.Models.ViewModels.Products.Creatines
{
    using System;
    using System.Collections.Generic;

    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Models.ViewModels.Reviews;

    public class DetailsCreatineViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool IsOnSale { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public ProductType ProductType { get; set; }

        public bool IsSuatableForVegans { get; set; }

        public string Directions { get; set; }

        public CreatineType Type { get; set; }

        public int Rating { get; set; }

        public ICollection<ReviewViewModel> Reviews { get; set; }
    }
}