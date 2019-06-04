namespace KeepFitStore.Models.Products
{
    using Enums;

    public class Protein : Supplement
    {
        public ProteinType Type{ get; set; }

        public double EnergyPerServing { get; set; }

        public double ProteinPerServing { get; set; }

        public double Carbohydrate { get; set; }

        public double Fat { get; set; }
    }
}