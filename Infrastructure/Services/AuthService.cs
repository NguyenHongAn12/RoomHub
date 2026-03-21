using Application.DTOs.Auth;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            IMemoryCache cache)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _cache = cache;
        }

        // =========================
        // LOGIN
        // =========================

        public async Task<SignInResult> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return SignInResult.Failed;

            // chưa verify email
            if (!user.EmailConfirmed)
                return SignInResult.NotAllowed;

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                request.Password,
                request.RememberMe,
                lockoutOnFailure: true);

            return result;
        }

        // =========================
        // REGISTER
        // =========================

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
                return false;

            var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            // Hash the password BEFORE saving to MemoryCache for security
            var dummyUser = new ApplicationUser { Email = request.Email, UserName = request.Email };
            var hashedPassword = _userManager.PasswordHasher.HashPassword(dummyUser, request.Password);

            var cacheData = new RegisterRequest
            {
                Email = request.Email,
                FullName = request.FullName,
                Password = hashedPassword, // Gán Hash Password vào đây
                Role = request.Role
            };

            // lưu info user tạm
            _cache.Set($"REGISTER_{request.Email}", cacheData, TimeSpan.FromMinutes(5));

            // lưu OTP
            _cache.Set($"OTP_{request.Email}", otp, TimeSpan.FromMinutes(5));

            await _emailService.SendOtpEmailAsync(request.Email, otp);

            return true;
        }

        // =========================
        // VERIFY OTP
        // =========================

        public async Task<bool> VerifyOtpAsync(VerifyOtpRequest request)
        {
            if (!_cache.TryGetValue($"OTP_{request.Email}", out string savedOtp))
                return false;

            if (savedOtp != request.Code)
                return false;

            if (!_cache.TryGetValue($"REGISTER_{request.Email}", out RegisterRequest registerData))
                return false;

            var user = new ApplicationUser
            {
                UserName = registerData.Email,
                Email = registerData.Email,
                FullName = registerData.FullName,
                EmailConfirmed = true,
                PasswordHash = registerData.Password // Lấy Hash Password từ Cache gán thẳng
            };

            // Tạo User mà không cần truyền Password dạng thô nữa
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
                return false;

            await _userManager.AddToRoleAsync(user, registerData.Role);

            _cache.Remove($"OTP_{request.Email}");
            _cache.Remove($"REGISTER_{request.Email}");

            await _signInManager.SignInAsync(user, false);

            return true;
        }

        // =========================
        // VALIDATION
        // =========================

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        // =========================
        // RESEND OTP
        // =========================

        public async Task<bool> ResendOtpAsync(string email)
        {
            // Kiểm tra xem session Register có còn tồn tại không
            if (!_cache.TryGetValue($"REGISTER_{email}", out RegisterRequest _))
                return false;

            // Generate new OTP
            var newOtp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            // Cập nhật lại thời gian sống của OTP (5 phút)
            _cache.Set($"OTP_{email}", newOtp, TimeSpan.FromMinutes(5));

            // Gửi lại email
            await _emailService.SendOtpEmailAsync(email, newOtp);

            return true;
        }

        // =========================

        // EXTERNAL LOGIN (GOOGLE / FACEBOOK)
        // =========================

        public async Task<bool> ExternalLoginCallbackAsync()
        {
            // Lấy thông tin từ provider (Google/Facebook)
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) return false;

            // Thử đăng nhập nếu user đã từng dùng social login này
            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true);

            if (signInResult.Succeeded)
                return true;

            // Lấy email từ claims do provider cung cấp
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return false;

            // Tìm user theo email (có thể đã đăng ký bằng email thông thường trước đó)
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // Tạo user mới từ thông tin social
                var fullName = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email;

                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    EmailConfirmed = true  // Email qua OAuth đã được xác thực bởi provider
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                    return false;

                // Gán role mặc định cho user mới đăng nhập qua social
                await _userManager.AddToRoleAsync(user, "Tenant");
            }

            // Liên kết tài khoản social với user trong hệ thống
            await _userManager.AddLoginAsync(user, info);

            // Đăng nhập
            await _signInManager.SignInAsync(user, isPersistent: false);

            return true;
        }

        // FORGOT PASSWORD
        // =========================

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            // Không tiết lộ user có tồn tại hay không (security best practice)
            if (user == null)
                return true;

            // Tạo token reset password từ ASP.NET Identity
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Lưu token vào cache 15 phút
            _cache.Set($"RESET_{email}", token, TimeSpan.FromMinutes(15));

            // Tạo OTP 6 số để hiển thị thân thiện hơn trên UI
            var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            _cache.Set($"RESET_OTP_{email}", otp, TimeSpan.FromMinutes(15));

            await _emailService.SendPasswordResetOtpAsync(email, otp);

            return true;
        }

        // =========================
        // VERIFY RESET OTP
        // =========================

        public Task<bool> VerifyResetOtpAsync(string email, string otp)
        {
            // Lấy OTP từ cache
            if (!_cache.TryGetValue($"RESET_OTP_{email}", out string savedOtp))
                return Task.FromResult(false);

            if (savedOtp != otp)
                return Task.FromResult(false);

            // Xóa OTP đã dùng, đánh dấu email đã xác thực OTP (cho phép đặt lại mật khẩu)
            _cache.Remove($"RESET_OTP_{email}");
            _cache.Set($"RESET_VERIFIED_{email}", true, TimeSpan.FromMinutes(10));

            return Task.FromResult(true);
        }

        // =========================
        // RESET PASSWORD
        // =========================

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            // Kiểm tra đã xác thực OTP chưa
            if (!_cache.TryGetValue($"RESET_VERIFIED_{request.Email}", out bool verified) || !verified)
                return IdentityResult.Failed(new IdentityError { Description = "Phiên làm việc đã hết hạn hoặc chưa xác thực OTP." });

            // Lấy token từ cache
            if (!_cache.TryGetValue($"RESET_{request.Email}", out string token))
                return IdentityResult.Failed(new IdentityError { Description = "Token đặt lại mật khẩu không hợp lệ hoặc đã hết hạn." });

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Không tìm thấy người dùng." });

            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (result.Succeeded)
            {
                // Nếu người dùng reset được mật khẩu bằng OTP (có nghĩa là họ sở hữu email này)
                // nhưng trước đó chưa xác thực email thì đây là dịp để xác thực luôn cho họ.
                if (!user.EmailConfirmed)
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }

                // Dọn cache
                _cache.Remove($"RESET_{request.Email}");
                _cache.Remove($"RESET_VERIFIED_{request.Email}");
            }

            return result;
        }

    }
}