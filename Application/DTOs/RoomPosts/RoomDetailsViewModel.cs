using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.RoomPosts
{
    public class RoomDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public decimal? SurfaceArea { get; set; }
        public string Address { get; set; } = null!;
        public string LocationDetails { get; set; } = null!; // Ward, District, City
        public RoomStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string RoomNumber { get; set; } = null!;
        public RoomType RoomType { get; set; }
        public bool IsFurnished { get; set; }
        public int MaxCapacity { get; set; }
        public int FloorNumber { get; set; }

        public List<RoomPhotoViewModel> Photos { get; set; } = new List<RoomPhotoViewModel>();

        // Deposit info (nullable/default)
        public decimal? DepositAmount { get; set; }

        // Amenities
        public List<AmenityViewModel> Amenities { get; set; } = new List<AmenityViewModel>();

        // Landlord Info
        public string LandlordId { get; set; } = null!;
        public string LandlordName { get; set; } = null!;
        public string? LandlordAvatarUrl { get; set; }
        public string? LandlordPhone { get; set; }
        public int LandlordJoinedYears { get; set; }
    }

    public class AmenityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? IconUrl { get; set; }
    }
}