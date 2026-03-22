using Domain.Enums;

namespace Application.DTOs.RoomPosts
{
    public class RoomSuggestionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public string? MainPhotoUrl { get; set; }
        public string Address { get; set; } = string.Empty;
        public RoomType RoomType { get; set; }
        public string RoomTypeLabel => RoomType switch
        {
            RoomType.Single => "Phòng đơn",
            RoomType.Double => "Phòng đôi",
            RoomType.Studio => "Căn hộ Studio",
            RoomType.Shared => "Phòng ở ghép",
            RoomType.Duplex => "Căn hộ Duplex",
            _ => "Khác"
        };
    }
}
