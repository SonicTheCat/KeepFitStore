namespace KeepFitStore.Models.InputModels.JobPositions
{
    using System.ComponentModel.DataAnnotations;

    using KeepFitStore.Models.Common;

    public class CreateJobPositionInputModel
    {
        [Required]
        [Display(Name = ModelsConstants.PositionName)]
        public string Name { get; set; }

        [Required]
        [Display(Name = ModelsConstants.StartingSalary)]
        [Range(typeof(decimal), ModelsConstants.StartingSalaryMinValue, ModelsConstants.StartingSalaryMaxValue, ErrorMessage = ModelsConstants.StartingSalaryErrorMessage)]
        public decimal Salary { get; set; }
    }
}