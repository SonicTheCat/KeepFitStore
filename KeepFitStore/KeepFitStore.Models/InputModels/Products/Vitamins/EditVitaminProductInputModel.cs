namespace KeepFitStore.Models.InputModels.Products.Vitamins
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    
    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Models.Common;

    public class EditVitaminProductInputModel
    {
        private const string VitamineTypeName = "Vitamine Type";

        public int Id { get; set; }

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
        [Display(Name = VitamineTypeName)]
        public VitaminType Type { get; set; }

        [Display(Name = ModelsConstants.UploadProducImageName)]
        public IFormFile Image { get; set; }
    }
}
