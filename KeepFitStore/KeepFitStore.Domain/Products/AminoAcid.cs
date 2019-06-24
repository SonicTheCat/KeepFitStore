namespace KeepFitStore.Domain.Products
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Enums;
    
    public class AminoAcid : Supplement
    {
        [Column(nameof(AminoAcidType))]
        public AminoAcidType Type { get; set; }

        [Column(nameof(AminoAcid) + nameof(EnergyPerServing))]
        public double EnergyPerServing { get; set; }

        [Column(nameof(AminoAcid) + nameof(Carbohydrate))]
        public double Carbohydrate { get; set; }

        [Column(nameof(AminoAcid) + nameof(Fat))]
        public double Fat { get; set; }
    }
}