namespace KeepFitStore.Domain.Products
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Enums;
    
    public class Protein : Supplement
    {
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
    }
}