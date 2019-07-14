namespace KeepFitStore.Models.InputModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    using KeepFitStore.Models.Common;

    public class CreateOrderAddressInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(ModelsConstants.StreetNameMaxLength,
           MinimumLength = ModelsConstants.StreetNameMinLength,
           ErrorMessage = ModelsConstants.StringErrorMessage)]
        [Display(Name = ModelsConstants.Street)]
        public string StreetName { get; set; }

        [Required]
        [Range(ModelsConstants.StreetNumberMinNumber, ModelsConstants.StreetNumberMaxNumber)]
        [Display(Name = ModelsConstants.StreetNumber)]
        public int StreetNumber { get; set; }

        public CreateOrderCityInputModel City { get; set; }
    }
}