namespace KeepFitStore.WEB.Areas.Administrator.Models.InputModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using KeepFitStore.Models.Enums;
    using KeepFitStore.WEB.Areas.Administrator.Models.Common;
    using Microsoft.AspNetCore.Http;

    public class CreateAminoAcidProducInputModel
    {
        private const string AminoAcidTypeName = "Amino Acid Type";

        [Required]
        [Display(Name = ModelsConstants.ProductBrandName)]
        public string Name { get; set; }

        [Required]
        [Range(typeof(decimal), ModelsConstants.PriceMinValue, ModelsConstants.PriceMaxValue, ErrorMessage = ModelsConstants.PriceErrorMessage)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(ModelsConstants.MaximumTextLenght)]
        public string Description { get; set; }

        [Required]
        [StringLength(ModelsConstants.MaximumTextLenght)]
        public string Directions { get; set; }

        [Display(Name = ModelsConstants.SuatableForVegansName)]
        public bool IsSuatableForVegans { get; set; }

        [Required]
        [Display(Name = AminoAcidTypeName)]
        public AminoAcidType Type { get; set; }

        [Required]
        [Display(Name = ModelsConstants.EnergyPerServingName)]
        public double EnergyPerServing { get; set; }

        [Required]
        public double Carbohydrate { get; set; }

        [Required]
        public double Fat { get; set; }

        [Required]
        [Display(Name = ModelsConstants.UploadProducImageName)]
        public IFormFile Image { get; set; }
    }
}