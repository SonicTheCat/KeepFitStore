namespace KeepFitStore.Models.InputModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    using KeepFitStore.Models.Common;

    public class CreateOrderCityInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(ModelsConstants.CityNameMaxLength,
            MinimumLength = ModelsConstants.CityNameMinLength,
            ErrorMessage = ModelsConstants.StringErrorMessage)]
        [Display(Name= ModelsConstants.City)]
        public string Name { get; set; }

        [Required]
        [Display(Name = ModelsConstants.Postcode)]
        public string PostCode { get; set; }
    }
}