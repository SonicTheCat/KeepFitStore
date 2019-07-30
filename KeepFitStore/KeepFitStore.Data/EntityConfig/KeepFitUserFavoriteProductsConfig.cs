namespace KeepFitStore.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using KeepFitStore.Domain;

    public class KeepFitUserFavoriteProductsConfig : IEntityTypeConfiguration<KeepFitUserFavoriteProducts>
    {
        public void Configure(EntityTypeBuilder<KeepFitUserFavoriteProducts> builder)
        {
            builder.HasKey(x => new { x.KeepFitUserId, x.ProductId });

            builder.HasOne(po => po.Product)
                   .WithMany(p => p.FavoriteProducts)
                   .HasForeignKey(po => po.ProductId);

            builder.HasOne(po => po.KeepFitUser)
                   .WithMany(x => x.FavoriteProducts)
                   .HasForeignKey(po => po.KeepFitUserId);
        }
    }
}