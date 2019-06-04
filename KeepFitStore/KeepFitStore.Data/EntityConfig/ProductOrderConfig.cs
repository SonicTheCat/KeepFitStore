namespace KeepFitStore.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ProductOrderConfig : IEntityTypeConfiguration<ProductOrder>
    {
        public void Configure(EntityTypeBuilder<ProductOrder> builder)
        {
            builder.HasKey(x => new { x.OrderId, x.ProductId });

            builder.HasOne(po => po.Product)
                   .WithMany(p => p.Orders)
                   .HasForeignKey(po => po.ProductId);

            builder.HasOne(po => po.Order)
                   .WithMany(o => o.Products)
                   .HasForeignKey(po => po.OrderId);
        }
    }
}