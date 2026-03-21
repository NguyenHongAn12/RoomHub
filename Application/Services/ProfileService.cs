using Application.DTOs.Profile;
using Application.Interfaces;
using Domain.Entities; // Thay namespace cho đúng
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Application.Interfaces.Services;

namespace Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        // Xóa IWebHostEnvironment đi, tầng này không được phép đụng tới ổ cứng web
        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ProfileDto> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new ProfileDto
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                AvatarUrl = user.AvatarUrl ?? "/images/default-avatar.png",
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                IsVerified = user.IsVerified,
                CreatedAt = user.CreatedAt,
                TotalFavoriteRooms = 0,
                TotalPostedRooms = 0
            };
        }

        public async Task<bool> UpdateUserProfileAsync(UpdateProfileDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return false;

            user.FullName = model.FullName;
            user.Address = model.Address;
            user.DateOfBirth = model.DateOfBirth;
            user.Gender = model.Gender;
            user.UpdatedAt = DateTime.UtcNow;

            // Bắt buộc dùng SetPhoneNumberAsync cho Identity thay vì gán trực tiếp
            if (user.PhoneNumber != model.PhoneNumber)
            {
                await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            }

            // Nếu Controller có truyền URL ảnh mới xuống thì mới cập nhật Avatar
            if (!string.IsNullOrEmpty(model.NewAvatarUrl))
            {
                user.AvatarUrl = model.NewAvatarUrl;
            }

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
