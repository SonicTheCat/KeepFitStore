namespace KeepFitStore.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using EntityConfig;
    using Models;
    using Models.Products;

    public class KeepFitDbContext : IdentityDbContext<KeepFitUser>
    {
        public KeepFitDbContext(DbContextOptions<KeepFitDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Supplement> Supplements { get; set; }

        public DbSet<Protein> Proteins { get; set; }

        public DbSet<Creatine> Creatines { get; set; }

        public DbSet<Vitamin> Vitamins { get; set; }

        public DbSet<AminoAcid> Aminos { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<BasketItem> BasketItems { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ProductOrder> ProductOrders{ get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AddressConfig());
            builder.ApplyConfiguration(new BasketItemConfig());
            builder.ApplyConfiguration(new ProductOrderConfig());
            builder.ApplyConfiguration(new KeepFitUserConfig());

            base.OnModelCreating(builder);
        }
    }
}