using Domain.Entities;
using System;

namespace RoomHub.Domain.Entities
{
    public class FavoriteRoom
    {
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}