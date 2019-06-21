namespace KeepFitStore.Models.Products
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Enums;
    
    public class Creatine : Supplement
    {
        [Column(nameof(CreatineType))]
        public CreatineType Type { get; set; }
    }
}