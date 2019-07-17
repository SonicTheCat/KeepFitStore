namespace KeepFitStore.Models.InputModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    using KeepFitStore.Models.Common;
    
    public class CreateOrderUserInputModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = ModelsConstants.UserFullName)]
        public string FullName { get; set; }

        [Required]
        [Display(Name = ModelsConstants.UserPhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}