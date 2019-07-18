namespace KeepFitStore.Models.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    
    public class DetailsOrdersViewModel
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal DeliveryPrice { get; set; }

        public string DeliveryType { get; set; }

        public string PaymentType { get; set; }

        public string Status { get; set; }

        public DetailsOrderUserViewModel KeepFitUser { get; set; }

        public DetailsOrdersAddressViewModel DeliveryAddress { get; set; }

        public ICollection<DetailsOrdersProductsViewModel> Products { get; set; }
    }
}