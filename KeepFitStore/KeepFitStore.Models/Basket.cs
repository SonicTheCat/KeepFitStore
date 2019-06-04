﻿namespace KeepFitStore.Models
{
    using System.Collections.Generic;

    public class Basket
    {
        public int Id { get; set; }

        public int KeepFitUserId { get; set; }
        public KeepFitUser KeepFitUser { get; set; }

        public ICollection<BasketItem> BasketItems { get; set; }
    }
}