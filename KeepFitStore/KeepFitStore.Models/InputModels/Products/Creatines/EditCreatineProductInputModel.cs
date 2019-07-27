namespace KeepFitStore.Models.InputModels.Products.Creatines
{
    using Microsoft.AspNetCore.Http;

    using System.ComponentModel.DataAnnotations;

    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Models.Common;

    public class EditCreatineProductInputModel
    {
        public const string CreatineTypeName = "Creatine Type";

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
        [Display(Name = CreatineTypeName)]
        public CreatineType Type { get; set; }

        [Display(Name = ModelsConstants.UploadProducImageName)]
        public IFormFile Image { get; set; }
    }
}
