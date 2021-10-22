using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsStand.Core.Entities;

namespace NewsStand.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Price)
                .HasColumnType("decimal(7,2)");

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(p => p.Producer)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.ProducerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
