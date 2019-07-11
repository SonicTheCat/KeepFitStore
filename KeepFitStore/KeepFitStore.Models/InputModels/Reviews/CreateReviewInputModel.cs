namespace KeepFitStore.Models.InputModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    using KeepFitStore.Models.Common;
    public class CreateReviewInputModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [Range(ModelsConstants.RatingMinValue , ModelsConstants.RatingMaxValue)]
        public int GivenRating { get; set; }

        public string KeepFitUserId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}