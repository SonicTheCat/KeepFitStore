namespace KeepFitStore.Models.ViewModels.Products.Proteins
{
    using System;
    using System.Collections.Generic;

    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Models.ViewModels.Reviews;

    public class DetailsProteinViewModel
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
  
        public ProteinType Type { get; set; }

        public double EnergyPerServing { get; set; }

        public double ProteinPerServing { get; set; }

        public double Carbohydrate { get; set; }

        public double Fat { get; set; }

        public double Salt { get; set; }

        public double Fibre { get; set; }

        public int Rating { get; set; }

        public ICollection<ReviewViewModel> Reviews { get; set; }
    }
}