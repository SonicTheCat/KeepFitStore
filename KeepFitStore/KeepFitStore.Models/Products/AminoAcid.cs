namespace KeepFitStore.Models.Products
{
    using Enums;

    public class AminoAcid : Supplement
    {
        public AminoAcidType Type { get; set; }

        public double EnergyPerServing { get; set; }

        public double Carbohydrate { get; set; }

        public double Fat { get; set; }
    }
}