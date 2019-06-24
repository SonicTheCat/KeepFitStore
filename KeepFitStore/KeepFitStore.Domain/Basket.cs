namespace KeepFitStore.Domain
{
    using System.Collections.Generic;

    public class Basket
    {
        public int Id { get; set; }

        public KeepFitUser KeepFitUser { get; set; }

        public ICollection<BasketItem> BasketItems { get; set; }
    }
}