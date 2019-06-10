namespace KeepFitStore.Data.EntityConfig
{
    using KeepFitStore.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class KeepFitUserConfig : IEntityTypeConfiguration<KeepFitUser>
    {
        public void Configure(EntityTypeBuilder<KeepFitUser> builder)
        {
            builder
                .HasOne(u => u.Basket)
                .WithOne(b => b.KeepFitUser)
                .HasForeignKey<KeepFitUser>(u => u.BasketId);
        }
    }
}