namespace KeepFitStore.Models.ViewModels.Orders
{
    using System;

    using KeepFitStore.Domain.Enums;

    public class AlllOrdersViewModel
    {
        public int Id { get; set; }

        public string KeepFitUserFullName { get; set; }

        public string KeepFitUserId { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public int ProductsCount { get; set; }
    }
}