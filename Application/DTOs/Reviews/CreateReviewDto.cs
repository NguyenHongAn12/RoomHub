using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Reviews
{
    public class CreateReviewDto
    {
        [Required]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Content cannot be empty")]
        [MinLength(1, ErrorMessage = "Content cannot be empty")]
        public string Content { get; set; } = string.Empty;

        public int? ParentReviewId { get; set; }
    }
}
