namespace KeepFitStore.Domain.Products
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Enums;
    
    public class Vitamin : Supplement
    {
        public Vitamin()
        {
            this.ProductType = ProductType.Vitamin;
        }

        [Column(nameof(VitaminType))]
        public VitaminType Type { get; set; }
    }
}