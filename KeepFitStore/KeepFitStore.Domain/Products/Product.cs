namespace KeepFitStore.Domain.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    using KeepFitStore.Domain.Enums;

    public abstract class Product
    {
        protected Product()
        {
            this.Orders = new HashSet<ProductOrder>();
            this.Reviews = new HashSet<Review>();
            this.CreatedOn = DateTime.UtcNow; 
            this.IsOnSale = false; 
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool IsOnSale { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public ProductType ProductType { get; set; }

        [NotMapped]
        public int Rating => this.Reviews.Sum(x => x.GivenRating) / this.Reviews.Count; 

        public ICollection<ProductOrder> Orders { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}