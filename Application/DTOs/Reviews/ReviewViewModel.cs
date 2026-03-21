using System;
using System.Collections.Generic;

namespace Application.DTOs.Reviews
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public string? UserAvatarUrl { get; set; }
        public List<ReviewViewModel> Replies { get; set; } = new();
    }
}
