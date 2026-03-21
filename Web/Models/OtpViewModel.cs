using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class OtpViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "OTP code is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be 6 digits")]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }
    }
}