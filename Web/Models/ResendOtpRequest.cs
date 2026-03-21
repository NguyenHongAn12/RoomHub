namespace Web.Models
{
    /// <summary>
    /// Dùng cho JSON body của các endpoint resend OTP qua AJAX (forgot password, v.v.)
    /// </summary>
    public class ResendOtpRequest
    {
        public string Email { get; set; }
    }
}
