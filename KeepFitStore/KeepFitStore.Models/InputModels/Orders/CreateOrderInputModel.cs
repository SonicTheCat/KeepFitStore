namespace KeepFitStore.Models.InputModels.Orders
{
    using System;
    using System.Collections.Generic;

    using KeepFitStore.Domain;
    using KeepFitStore.Domain.Enums;

    public class CreateOrderInputModel
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal DeliveryPrice { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public PaymentType PaymentType { get; set; }

        public OrderStatus Status { get; set; }

        public CreateOrderUserInputModel User { get; set; }

        public CreateOrderAddressInputModel DeliveryAddress { get; set; }

        public ICollection<CreateOrderProductInputModel> Products { get; set; } = new HashSet<CreateOrderProductInputModel>(); 
    }
}