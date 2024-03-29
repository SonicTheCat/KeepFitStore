﻿namespace KeepFitStore.Domain.Products
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Enums;
    
    public class Protein : Supplement
    {
        public Protein()
        {
            this.ProductType = ProductType.Protein; 
        }

        [Column(nameof(ProteinType))]
        public ProteinType Type{ get; set; }

        [Column(nameof(Protein) + nameof(EnergyPerServing))]
        public double EnergyPerServing { get; set; }

        [Column(nameof(Protein) + nameof(ProteinPerServing))]
        public double ProteinPerServing { get; set; }

        [Column(nameof(Protein) + nameof(Carbohydrate))]
        public double Carbohydrate { get; set; }

        [Column(nameof(Protein) + nameof(Fat))]
        public double Fat { get; set; }

        [Column(nameof(Protein) + nameof(Salt))]
        public double Salt { get; set; }

        [Column(nameof(Protein) + nameof(Fibre))]
        public double Fibre { get; set; }
    }
}