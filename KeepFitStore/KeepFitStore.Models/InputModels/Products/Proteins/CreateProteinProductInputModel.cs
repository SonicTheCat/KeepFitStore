namespace KeepFitStore.Models.InputModels.Products.Proteins
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Models.Common;

    public class CreateProteinProductInputModel
    {
        private const string ProteinTypeName = "Protein type";

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

        [Required]
        [Display(Name = ProteinTypeName)]
        public ProteinType Type { get; set; }

        [Required]
        [Display(Name = ModelsConstants.EnergyPerServingName)]
        public double EnergyPerServing { get; set; }

        [Required]
        [Display(Name = ModelsConstants.ProteinPerServingName)]
        public double ProteinPerServing { get; set; }

        [Required]
        public double Carbohydrate { get; set; }

        [Required]
        public double Fat { get; set; }

        [Required]
        public double Salt { get; set; }

        [Required]
        public double Fibre { get; set; }

        [Display(Name = ModelsConstants.SuatableForVegansName)]
        public bool IsSuatableForVegans { get; set; }

        [Required]
        [Display(Name = ModelsConstants.UploadProducImageName)]
        public IFormFile Image { get; set; }
    }
}