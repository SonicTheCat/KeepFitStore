namespace KeepFitStore.Domain.Products
{
    using KeepFitStore.Domain.Enums;
    using System.Collections.Generic;

    public abstract class Product
    {
        protected Product()
        {
            this.Orders = new HashSet<ProductOrder>();
            this.IsOnSale = false; 
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool IsOnSale { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public ProductType ProductType { get; set; }

        public ICollection<ProductOrder> Orders { get; set; }
    }
}