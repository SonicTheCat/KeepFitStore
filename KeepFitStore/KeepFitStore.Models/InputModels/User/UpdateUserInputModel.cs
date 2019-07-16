namespace KeepFitStore.Models.InputModels.User
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateUserInputModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}