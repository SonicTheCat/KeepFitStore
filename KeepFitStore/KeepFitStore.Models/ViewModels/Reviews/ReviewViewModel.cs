namespace KeepFitStore.Models.ViewModels.Reviews
{
    using System;

    using KeepFitStore.Domain;

    public class ReviewViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int GivenRating { get; set; }

        public int ProductId { get; set; }

        //public Product Product { get; set; }

        public string KeepFitUserId { get; set; }

        public string UserFullName { get; set; }

        public DateTime PublishedOn { get; set; }
    }
}