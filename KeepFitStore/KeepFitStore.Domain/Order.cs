namespace KeepFitStore.Domain
{
    using System;
    using System.Collections.Generic;

    using Enums;

    public class Order
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal DeliveryPrice { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public PaymentType PaymentType { get; set; }

        public OrderStatus Status { get; set; }

        public string KeepFitUserId { get; set; }
        public KeepFitUser KeepFitUser { get; set; }

        public int? AddressId { get; set; }
        public Address DeliveryAddress { get; set; }

        public ICollection<ProductOrder> Products { get; set; }
    }
}