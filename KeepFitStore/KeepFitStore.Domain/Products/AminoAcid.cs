namespace KeepFitStore.Domain.Products
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Enums;
    
    public class AminoAcid : Supplement
    {
        public AminoAcid()
        {
            this.ProductType = ProductType.Amino;
        }

        [Column(nameof(AminoAcidType))]
        public AminoAcidType Type { get; set; }

        [Column(nameof(AminoAcid) + nameof(EnergyPerServing))]
        public double EnergyPerServing { get; set; }

        [Column(nameof(AminoAcid) + nameof(Carbohydrate))]
        public double Carbohydrate { get; set; }

        [Column(nameof(AminoAcid) + nameof(Fat))]
        public double Fat { get; set; }

        [Column(nameof(AminoAcid) + nameof(ProteinPerServing))]
        public double ProteinPerServing { get; set; }

        [Column(nameof(AminoAcid) + nameof(Salt))]
        public double Salt { get; set; }
    }
}