namespace KeepFitStore.Domain
{
    using System.Collections.Generic;

    public class City
    {
        public City()
        {
            this.Addresses = new HashSet<Address>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string PostCode { get; set; }

        public ICollection<Address> Addresses{ get; set; }
    }
}