using System;

namespace Domain.Entities
{
    public class RoomPhoto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Url { get; set; } = null!;
        public string PublicId { get; set; } = null!;
        public bool IsMain { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public virtual Room Room { get; set; } = null!;
    }
}
