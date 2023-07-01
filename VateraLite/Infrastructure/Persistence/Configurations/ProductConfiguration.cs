using VateraLite.Domain.Entities;

namespace VateraLite.Infrastructure.Persistence.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
