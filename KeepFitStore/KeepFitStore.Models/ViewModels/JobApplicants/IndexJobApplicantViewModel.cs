namespace KeepFitStore.Models.ViewModels.JobApplicants
{
    public class IndexJobApplicantViewModel
    {
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Middlename { get; set; }

        public string Lastname { get; set; }

        public string PhoneNumber { get; set; }

        public int Age { get; set; }

        public bool ExperienceInSelling { get; set; }

        public string Bio { get; set; }

        public int PositionId { get; set; }

        public string PositionName { get; set; }

        public string ImageUrl { get; set; }
    }
}