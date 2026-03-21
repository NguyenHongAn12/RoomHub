public interface IEmailService
{
    Task SendOtpEmailAsync(string email, string otp);

    Task SendPasswordResetOtpAsync(string email, string otp);
}