namespace KeepFitStore.Models.ViewModels.Orders
{
    using System;

    using KeepFitStore.Domain.Enums;

    public class IndexOrdersViewModel
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public int ProductsCount { get; set; }
    }
}