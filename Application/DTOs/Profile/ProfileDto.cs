namespace Application.DTOs.Profile
{
    public class ProfileDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }

        public int TotalFavoriteRooms { get; set; }
        public int TotalPostedRooms { get; set; }
    }
}
