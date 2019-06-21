namespace KeepFitStore.Models.Products
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Enums;
    
    public class Vitamin : Supplement
    {
        [Column(nameof(VitaminType))]
        public VitaminType Type { get; set; }
    }
}