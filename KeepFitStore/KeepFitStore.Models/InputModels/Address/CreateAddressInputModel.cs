namespace KeepFitStore.Models.InputModels.Address
{
    using System.ComponentModel.DataAnnotations;

    using KeepFitStore.Models.Common;

    public class CreateAddressInputModel
    {
        [Required]
        [StringLength(ModelsConstants.CityNameMaxLength,
            MinimumLength = ModelsConstants.CityNameMinLength,
            ErrorMessage = ModelsConstants.StringErrorMessage)]
        public string CityName { get; set; }

        [Required]
        public string PostCode { get; set; }


        [Required]
        [StringLength(ModelsConstants.StreetNameMaxLength,
            MinimumLength = ModelsConstants.StreetNameMinLength,
            ErrorMessage = ModelsConstants.StringErrorMessage)]
        public string StreetName { get; set; }

        [Required]
        [Range(ModelsConstants.StreetNumberMinNumber, ModelsConstants.StreetNumberMaxNumber)]
        public int StreetNumber { get; set; }
    }
}