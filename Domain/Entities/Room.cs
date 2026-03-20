using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int FloorId { get; set; }
        public string RoomNumber { get; set; } = null!;
        public RoomType RoomType { get; set; }
        public int MaxCapacity { get; set; } = 2;
        public decimal? SurfaceArea { get; set; }
        public decimal BasePrice { get; set; }
        public string? Description { get; set; }
        public bool IsFurnished { get; set; } = true;
        public RoomStatus Status { get; set; }
        
        // Post fields
        public string LandlordId { get; set; } = null!;
        public string Title { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public virtual ApplicationUser Landlord { get; set; } = null!;
        public virtual Floor Floor { get; set; } = null!;
        public virtual ICollection<RoomPhoto> RoomPhotos { get; set; } = new List<RoomPhoto>();
        public virtual ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
        public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();
        public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
        public virtual ICollection<MaintenanceTicket> MaintenanceTickets { get; set; } = new List<MaintenanceTicket>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<SearchHistory> SearchHistories { get; set; } = new List<SearchHistory>();
        public virtual ICollection<BookingHistory> BookingHistories { get; set; } = new List<BookingHistory>();
    }
}
