using VateraLite.Domain.Entities;

namespace VateraLite.Infrastructure.Persistence.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.ApplicationUser)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
