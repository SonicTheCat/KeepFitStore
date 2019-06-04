namespace KeepFitStore.Models.Products
{
    using Enums;

    public class Vitamin : Supplement
    {
        public VitaminType Type { get; set; }
    }
}