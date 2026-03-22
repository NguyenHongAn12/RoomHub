using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FullName).HasMaxLength(256).IsRequired();
            builder.Property(u => u.Gender).HasMaxLength(20);
            builder.Property(u => u.Address).HasMaxLength(512);
            builder.Property(u => u.AvatarUrl).HasMaxLength(512);
            builder.Property(u => u.PhoneNumber).HasMaxLength(50);
            builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(u => u.IsDeleted).HasDefaultValue(false);
            builder.Property(u => u.IsBanned).HasDefaultValue(false);
            builder.Property(u => u.IsVerified).HasDefaultValue(false);
            builder.Property(u => u.EmailConfirmed).HasDefaultValue(false);
            builder.Property(u => u.PhoneNumberConfirmed).HasDefaultValue(false);
            builder.Property(u => u.TwoFactorEnabled).HasDefaultValue(false);
            builder.Property(u => u.LockoutEnabled).HasDefaultValue(true);
            builder.Property(u => u.AccessFailedCount).HasDefaultValue(0);
            builder.Property(u => u.ReviewBlockedUntil).IsRequired(false);
        }
    }
}
