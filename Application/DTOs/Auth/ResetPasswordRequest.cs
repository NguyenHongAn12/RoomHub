namespace Application.DTOs.Auth
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }

        /// <summary>
        /// Token được generate bởi ASP.NET Identity, lưu trong MemoryCache sau khi OTP xác thực thành công.
        /// </summary>
        public string ResetToken { get; set; }
    }
}
