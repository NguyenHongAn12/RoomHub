using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }
}
