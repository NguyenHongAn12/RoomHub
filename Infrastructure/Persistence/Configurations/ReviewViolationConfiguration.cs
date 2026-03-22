using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ReviewViolationConfiguration : IEntityTypeConfiguration<ReviewViolation>
    {
        public void Configure(EntityTypeBuilder<ReviewViolation> builder)
        {
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Content).IsRequired().HasMaxLength(1000);
            builder.Property(v => v.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
