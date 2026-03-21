using Application.DTOs.Profile;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(IProfileService profileService, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            _profileService = profileService;
            _env = env;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _profileService.GetUserProfileAsync(userId);
            if (profile == null) return NotFound();
            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _profileService.GetUserProfileAsync(userId);
            if (profile == null) return NotFound();

            return View(new UpdateProfileDto
            {
                Id = profile.Id,
                FullName = profile.FullName,
                PhoneNumber = profile.PhoneNumber,
                Address = profile.Address,
                DateOfBirth = profile.DateOfBirth,
                Gender = profile.Gender,
                CurrentAvatarUrl = profile.AvatarUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bắt file ảnh tách rời so với DTO
        public async Task<IActionResult> Edit(UpdateProfileDto model, IFormFile? NewAvatar)
        {
            if (!ModelState.IsValid) return View(model);

            // 1. NGHIỆP VỤ UPLOAD ẢNH (CHỈ NẰM Ở TẦNG WEB)
            if (NewAvatar != null && NewAvatar.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "avatars");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(NewAvatar.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await NewAvatar.CopyToAsync(fileStream);
                }

                // Gán URL vào DTO để ném xuống Service
                model.NewAvatarUrl = $"/images/avatars/{uniqueFileName}";
            }

            // 2. GIAO CHO SERVICE CẬP NHẬT DATABASE
            model.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _profileService.UpdateUserProfileAsync(model);

            if (result)
            {
                TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Lỗi khi cập nhật cơ sở dữ liệu.");
            return View(model);
        }

        // ================== CHANGE PASSWORD ==================
        [HttpGet]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);
            if (user == null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }

        // ================== SAVED ROOMS ====================
        [HttpGet]
        public async Task<IActionResult> MySavedRooms()
        {
            // TODO: Inject ISavedRoomService + query by userId when available
            return View();
        }
    }
}
