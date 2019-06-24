namespace KeepFitStore.Domain.Products
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Enums;
    
    public class Creatine : Supplement
    {
        public Creatine()
        {
            this.ProductType = ProductType.Creatine;
        }

        [Column(nameof(CreatineType))]
        public CreatineType Type { get; set; }
    }
}