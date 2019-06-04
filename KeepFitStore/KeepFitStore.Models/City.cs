namespace KeepFitStore.Models
{
    using System.Collections.Generic;

    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PostCode { get; set; }

        public ICollection<Address> Addresses{ get; set; }
    }
}