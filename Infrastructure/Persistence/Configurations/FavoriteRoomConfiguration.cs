using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomHub.Domain.Entities;


namespace RoomHub.Infrastructure.Persistence.Configurations
{
    public class FavoriteRoomConfiguration : IEntityTypeConfiguration<FavoriteRoom>
    {
        public void Configure(EntityTypeBuilder<FavoriteRoom> builder)
        {
            builder.ToTable("FavoriteRooms");

            builder.HasKey(f => new { f.UserId, f.RoomId });

            builder.HasOne(f => f.User)
                .WithMany() 
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Room)
                .WithMany() 
                .HasForeignKey(f => f.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}