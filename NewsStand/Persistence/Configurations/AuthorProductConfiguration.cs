using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsStand.Core.Entities;

namespace NewsStand.Persistence.Configurations
{
    public class AuthorProductConfiguration : IEntityTypeConfiguration<AuthorProduct>
    {
        public void Configure(EntityTypeBuilder<AuthorProduct> builder)
        {
            builder.ToTable("AuthorProducts");

            builder.HasKey(ap => new { ap.AuthorId, ap.ProductId });

            builder.HasOne(ap => ap.Author)
                .WithMany(a => a.AuthorProducts)
                .HasForeignKey(ap => ap.AuthorId);

            builder.HasOne(ap => ap.Product)
                .WithMany(p => p.AuthorProducts)
                .HasForeignKey(ap => ap.ProductId);
        }
    }
}
