namespace KeepFitStore.Models.InputModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using KeepFitStore.Domain;
    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Models.Common;

    public class CreateOrderInputModel
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal DeliveryPrice { get; set; }

        [Required]
        public string DeliveryType { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        public OrderStatus Status { get; set; }

        public CreateOrderUserInputModel User { get; set; }

        public CreateOrderAddressInputModel DeliveryAddress { get; set; }

        public ICollection<CreateOrderProductInputModel> Products { get; set; } = new HashSet<CreateOrderProductInputModel>(); 
    }
}