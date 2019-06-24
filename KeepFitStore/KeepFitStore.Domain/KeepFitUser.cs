namespace KeepFitStore.Domain
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class KeepFitUser : IdentityUser
    {
        public string FullName { get; set; }

        public int? AddressId { get; set; }
        public Address Address { get; set; }

        public int BasketId { get; set; }
        public Basket Basket { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}