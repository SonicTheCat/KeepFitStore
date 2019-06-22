namespace KeepFitStore.WEB.Areas.Administrator.Models.InputModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using KeepFitStore.Models.Enums;

    public class CreateCreatineProductInputModel
    {
        private const int MaximumTextLenght = 500;

        private const string ProductBrandName = "Brand";

        private const string PriceMinValue = "0.01";

        private const string PriceMaxValue = "79228162514264337593543950335";

        private const string PriceErrorMessage = "Price is not in the allowed range!";

        private const string SuatableForVegansName = "Suatable for vegans?";

        private const string CreatineTypeName = "Creatine Type";

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

        [Display(Name = SuatableForVegansName)]
        public bool IsSuatableForVegans { get; set; }

        [Required]
        [Display(Name = CreatineTypeName)]
        public CreatineType Type { get; set; }
    }
}
