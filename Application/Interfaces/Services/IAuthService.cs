using Application.DTOs.Auth;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Microsoft.AspNetCore.Identity.SignInResult> LoginAsync(LoginRequest request);

        Task<bool> RegisterAsync(RegisterRequest request);

        Task<bool> VerifyOtpAsync(VerifyOtpRequest request);

        Task<bool> CheckEmailExistsAsync(string email);

        Task<bool> ResendOtpAsync(string email);

        // SOCIAL LOGIN
        Task<bool> ExternalLoginCallbackAsync();

        // FORGOT PASSWORD
        Task<bool> ForgotPasswordAsync(string email);

        Task<bool> VerifyResetOtpAsync(string email, string otp);

        Task<Microsoft.AspNetCore.Identity.IdentityResult> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
