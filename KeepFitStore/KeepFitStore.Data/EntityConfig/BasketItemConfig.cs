namespace KeepFitStore.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class BasketItemConfig : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.BasketId });

            builder.HasOne(x => x.Basket)
                   .WithMany(b => b.BasketItems)
                   .HasForeignKey(x => x.BasketId);
        }
    }
}