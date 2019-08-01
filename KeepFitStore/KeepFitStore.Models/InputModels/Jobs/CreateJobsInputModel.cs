namespace KeepFitStore.Models.InputModels.Jobs
{
    using System.ComponentModel.DataAnnotations;

    using KeepFitStore.Models.Common;

    public class CreateJobsInputModel
    {
        [Required]
        public string Firstname { get; set; }

        public string Middlename { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        [Display(Name = ModelsConstants.UserPhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [Range(ModelsConstants.JobApplicantMinAge, ModelsConstants.JobApplicantMaxAge, ErrorMessage = ModelsConstants.AgeErrorMessage)]
        public int Age { get; set; }

        public bool ExperienceInSelling { get; set; }

        [Required]
        [Display(Name = ModelsConstants.Biography)]
        [StringLength(ModelsConstants.MaximumTextLenght, MinimumLength = ModelsConstants.MinimumTextLenght, ErrorMessage = ModelsConstants.BiographyErrorMessage)]
        public string Bio { get; set; }
    
        public string Position { get; set; }
    }
}