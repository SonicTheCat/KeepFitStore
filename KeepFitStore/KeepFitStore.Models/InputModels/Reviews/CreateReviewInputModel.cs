namespace KeepFitStore.Models.InputModels.Reviews
{
    public class CreateReviewInputModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int GivenRating { get; set; }

        public string KeepFitUserId { get; set; }

        public int ProductId { get; set; }
    }
}