using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Application.Interfaces.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendOtpEmailAsync(string email, string otp)
    {
        using var smtpClient = new SmtpClient(_config["EmailSettings:Host"])
        {
            Port = int.Parse(_config["EmailSettings:Port"]),
            Credentials = new NetworkCredential(
                _config["EmailSettings:Email"],
                _config["EmailSettings:Password"]
            ),
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_config["EmailSettings:Email"]),
            Subject = "RoomHub Email Verification",
            Body = $"Your OTP verification code is: {otp}",
            IsBodyHtml = false
        };

        mail.To.Add(email);

        try
        {
            await smtpClient.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to send OTP email", ex);
        }
    }

    public async Task SendPasswordResetOtpAsync(string email, string otp)
    {
        using var smtpClient = new SmtpClient(_config["EmailSettings:Host"])
        {
            Port = int.Parse(_config["EmailSettings:Port"]),
            Credentials = new NetworkCredential(
                _config["EmailSettings:Email"],
                _config["EmailSettings:Password"]
            ),
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_config["EmailSettings:Email"]),
            Subject = "RoomHub - Đặt lại mật khẩu",
            Body = $"Mã OTP đặt lại mật khẩu của bạn là: <b>{otp}</b><br/>" +
                   $"Mã có hiệu lực trong 15 phút. Không chia sẻ mã này với ai.",
            IsBodyHtml = true
        };

        mail.To.Add(email);

        try
        {
            await smtpClient.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to send password reset email", ex);
        }
    }
}