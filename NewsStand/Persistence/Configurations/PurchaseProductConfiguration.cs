using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsStand.Core.Entities;

namespace NewsStand.Persistence.Configurations
{
    public class PurchaseProductConfiguration : IEntityTypeConfiguration<PurchaseProduct>
    {
        public void Configure(EntityTypeBuilder<PurchaseProduct> builder)
        {
            builder.ToTable("PurchaseProducts");

            builder.Property(pp => pp.Price)
                .HasColumnType("decimal(7,2)");

            builder.HasOne(pp => pp.Product)
                .WithMany()
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
