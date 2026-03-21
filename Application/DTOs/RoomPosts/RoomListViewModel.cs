using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.RoomPosts
{
    public class RoomListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public decimal? SurfaceArea { get; set; }
        public string Address { get; set; } = null!;
        public RoomStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RoomNumber { get; set; } = null!;
        public RoomType RoomType { get; set; }
        public int AmenityCount { get; set; }
    }
}