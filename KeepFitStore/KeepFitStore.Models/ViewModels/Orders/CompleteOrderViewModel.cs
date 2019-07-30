namespace KeepFitStore.Models.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;

    using KeepFitStore.Domain.Enums;
    
    public class CompleteOrderViewModel
    {
        public int Id { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public PaymentType PaymentType { get; set; }

        public decimal TotalPrice { get; set; }

        public string KeepFitUserId { get; set; }

        public string KeepFitUserFullName { get; set; }

        public IEnumerable<CompleteOrderProductsViewModel> Products { get; set; }
    }
}