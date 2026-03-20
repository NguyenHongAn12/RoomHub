using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class RoomPhotoConfiguration : IEntityTypeConfiguration<RoomPhoto>
    {
        public void Configure(EntityTypeBuilder<RoomPhoto> builder)
        {
            builder.ToTable("RoomPhotos");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.PublicId)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne(x => x.Room)
                .WithMany(r => r.RoomPhotos)
                .HasForeignKey(x => x.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
