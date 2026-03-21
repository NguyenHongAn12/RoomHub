using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Profile
{
    public class UpdateProfileDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FullName { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        public string? CurrentAvatarUrl { get; set; }
        public string? NewAvatarUrl { get; set; }
    }
}
