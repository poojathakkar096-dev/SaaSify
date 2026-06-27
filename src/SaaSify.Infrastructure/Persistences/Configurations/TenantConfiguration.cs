using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaaSify.Domain.Entity;

namespace SaaSify.Infrastructure.Persistences.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenants");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.SubscriptionPlan)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Free");

            builder.Property(t => t.IsActive)
                .HasDefaultValue(true);

            // One Tenant has many Users
            builder.HasMany(t => t.Users)
                .WithOne(u => u.Tenant)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index on Email for faster lookups
            builder.HasIndex(t => t.Email).IsUnique();
        }
    }
}
