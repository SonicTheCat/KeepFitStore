namespace KeepFitStore.Domain
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class KeepFitUser : IdentityUser
    {
        public KeepFitUser()
        {
            this.Orders = new HashSet<Order>();
            this.Reviews = new HashSet<Review>();
            this.FavoriteProducts = new HashSet<KeepFitUserFavoriteProducts>();
        }

        public string FullName { get; set; }

        public int? AddressId { get; set; }
        public Address Address { get; set; }

        public int BasketId { get; set; }
        public Basket Basket { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<Review> Reviews{ get; set; }

        public ICollection<KeepFitUserFavoriteProducts> FavoriteProducts{ get; set; }
    }
}