namespace KeepFitStore.WEB.Areas.Administrator.Models.InputModels.Products
{
    using KeepFitStore.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class CreateProteinProductInputModel
    {
        private const string ProteinTypeName = "Protein type";

        private const string EnergyPerServingName = "Energy per serving";

        private const string ProteinPerServingName = "Protein per serving";

        private const int MaximumTextLenght = 500;

        private const string ProductBrandName = "Brand";

        private const string PriceMinValue = "0.01";

        private const string PriceMaxValue = "79228162514264337593543950335";
        
        private const string PriceErrorMessage = "Price is not in the allowed range!";

        private const string SuatableForVegansName = "Suatable for vegans?";

        [Required]
        [Display(Name = ProductBrandName)]
        public string Name { get; set; }

        [Required]
        [Range(typeof(decimal), PriceMinValue, PriceMaxValue, ErrorMessage = PriceErrorMessage)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(MaximumTextLenght)]
        public string Description { get; set; }

        [Required]
        [StringLength(MaximumTextLenght)]
        public string Directions { get; set; }

        [Required]
        [Display(Name = ProteinTypeName)]
        public ProteinType Type { get; set; }

        [Required]
        [Display(Name = EnergyPerServingName)]
        public double EnergyPerServing { get; set; }

        [Required]
        [Display(Name = ProteinPerServingName)]
        public double ProteinPerServing { get; set; }

        [Required]
        public double Carbohydrate { get; set; }

        [Required]
        public double Fat { get; set; }

        [Display(Name = SuatableForVegansName)]
        public bool IsSuatableForVegans { get; set; }
    }
}