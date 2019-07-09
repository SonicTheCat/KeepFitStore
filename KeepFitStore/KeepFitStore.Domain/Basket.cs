namespace KeepFitStore.Domain
{
    using System.Collections.Generic;

    public class Basket
    {
        public Basket()
        {
            this.BasketItems = new HashSet<BasketItem>();
        }

        public int Id { get; set; }

        public KeepFitUser KeepFitUser { get; set; }

        public ICollection<BasketItem> BasketItems { get; set; }
    }
}