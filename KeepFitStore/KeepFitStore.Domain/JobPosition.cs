namespace KeepFitStore.Domain
{
    using System.Collections.Generic;

    public class JobPosition
    {
        public JobPosition()
        {
            this.Applicants = new HashSet<JobApplicant>(); 
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Salary { get; set; }

        public ICollection<JobApplicant> Applicants { get; set; }
    }
}