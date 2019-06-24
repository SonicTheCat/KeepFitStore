namespace KeepFitStore.Models.InputModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Models.Common;
    using Microsoft.AspNetCore.Http;

    public class CreateCreatineProductInputModel
    {
        public const string CreatineTypeName = "Creatine Type";

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
        [Display(Name = CreatineTypeName)]
        public CreatineType Type { get; set; }

        [Required]
        [Display(Name = ModelsConstants.UploadProducImageName)]
        public IFormFile Image { get; set; }
    }
}