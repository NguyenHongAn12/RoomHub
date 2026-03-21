using Application.DTOs.Auth;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(IAuthService authService, SignInManager<ApplicationUser> signInManager)
        {
            _authService = authService;
            _signInManager = signInManager;
        }

        // =========================
        // LOGIN
        // =========================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(new LoginRequest
            {
                Email = model.Email,
                Password = model.Password,
                RememberMe = model.RememberMe
            });

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "RoomPosts");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa tạm thời do thử sai quá nhiều lần. Vui lòng thử lại sau.");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError("", "Tài khoản của bạn chưa được phép đăng nhập (có thể do chưa xác thực email).");
            }
            else
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không chính xác.");
            }

            return View(model);
        }

        // =========================
        // REGISTER
        // =========================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.RegisterAsync(new RegisterRequest
            {
                Email = model.Email,
                Password = model.Password,
                FullName = model.FullName,
                Role = model.Role
            });

            if (!result)
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                return View(model);
            }

            // lưu email để verify OTP
            TempData["VerifyEmail"] = model.Email;

            return RedirectToAction(nameof(VerifyOtp));
        }

        // =========================
        // VERIFY OTP
        // =========================

        [HttpGet]
        public IActionResult VerifyOtp()
        {
            var email = TempData["VerifyEmail"]?.ToString();

            if (string.IsNullOrEmpty(email))
                return RedirectToAction(nameof(Register));

            var model = new OtpViewModel
            {
                Email = email
            };

            TempData.Keep("VerifyEmail");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOtp(OtpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var request = new VerifyOtpRequest
            {
                Email = model.Email,
                Code = model.Code
            };
            var result = await _authService.VerifyOtpAsync(request);

            if (!result)
            {
                ModelState.AddModelError("", "OTP không đúng hoặc đã hết hạn");
                return View(model);
            }

            return RedirectToAction("VerifySuccess");
        }

        public IActionResult VerifySuccess()
        {
            return View();
        }

        // =========================
        // FORGOT PASSWORD
        // =========================

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Luôn trả về true dù email có tồn tại hay không (security: không lộ info)
            await _authService.ForgotPasswordAsync(model.Email);

            // Lưu email để dùng ở bước verify OTP tiếp theo
            TempData["ResetEmail"] = model.Email;

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            TempData.Keep("ResetEmail");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendForgotPasswordOtp([FromBody] ResendOtpRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Email))
                return Json(new { success = false, message = "Email không hợp lệ." });

            await _authService.ForgotPasswordAsync(request.Email);

            return Json(new { success = true, message = "OTP mới đã được gửi." });
        }

        // =========================
        // VERIFY RESET OTP
        // =========================

        [HttpGet]
        public IActionResult VerifyResetOtp()
        {
            var email = TempData["ResetEmail"]?.ToString();

            if (string.IsNullOrEmpty(email))
                return RedirectToAction(nameof(ForgotPassword));

            TempData.Keep("ResetEmail");

            return View(new OtpViewModel { Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyResetOtp(OtpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.VerifyResetOtpAsync(model.Email, model.Code);

            if (!result)
            {
                ModelState.AddModelError("", "OTP không đúng hoặc đã hết hạn.");
                return View(model);
            }

            TempData["ResetEmail"] = model.Email;
            return RedirectToAction(nameof(ResetPassword));
        }

        // =========================
        // RESET PASSWORD
        // =========================

        [HttpGet]
        public IActionResult ResetPassword()
        {
            var email = TempData["ResetEmail"]?.ToString();

            if (string.IsNullOrEmpty(email))
                return RedirectToAction(nameof(ForgotPassword));

            TempData.Keep("ResetEmail");

            return View(new ResetPasswordViewModel { Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.ResetPasswordAsync(new Application.DTOs.Auth.ResetPasswordRequest
            {
                Email = model.Email,
                NewPassword = model.NewPassword
            });

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            return RedirectToAction(nameof(Login));
        }

        // =========================
        // VALIDATION
        // =========================

        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Json(new { exists = false });

            var exists = await _authService.CheckEmailExistsAsync(email);
            return Json(new { exists });
        }

        [HttpPost]
        public async Task<IActionResult> ResendOtp(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Json(new { success = false, message = "Email không hợp lệ." });

            var result = await _authService.ResendOtpAsync(email);

            if (result)
                return Json(new { success = true, message = "Mã OTP mới đã được gửi vào email của bạn." });

            return Json(new { success = false, message = "Phiên đăng ký đã hết hạn. Vui lòng đăng ký lại." });
        }

        // =========================
        // EXTERNAL LOGIN (GOOGLE / FACEBOOK)
        // =========================

        [HttpGet]
        public IActionResult LoginGoogle()
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        [HttpGet]
        public IActionResult LoginFacebook()
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return Challenge(properties, "Facebook");
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var result = await _authService.ExternalLoginCallbackAsync();

            if (!result)
            {
                TempData["Error"] = "Đăng nhập mạng xã hội thất bại. Vui lòng thử lại.";
                return RedirectToAction(nameof(Login));
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // 1. Thực hiện xóa phiên đăng nhập (Xóa Cookie xác thực)

            // CÁCH 1: Nếu bạn dùng chuẩn ASP.NET Core Identity (FE-01)
            if (_signInManager != null)
            {
                await _signInManager.SignOutAsync();
            }
            else
            {
                // CÁCH 2: Nếu bạn code Cookie Authentication thủ công
                await HttpContext.SignOutAsync();
                // Hoặc: await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            // Xóa thêm Session nếu dự án của bạn có lưu trữ giỏ hàng/dữ liệu tạm trong Session
            // HttpContext.Session.Clear();

            // 2. Chuyển hướng người dùng về lại trang chủ (Controller: RoomPosts, Action: Index)
            return RedirectToAction("Index", "RoomPosts");
        }
    }
}