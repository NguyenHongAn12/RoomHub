using Application.DTOs.Reviews;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IGeminiModerationService _moderationService;
        private readonly IReviewViolationRepository _violationRepository;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public ReviewService(
            IReviewRepository reviewRepository, 
            IGeminiModerationService moderationService,
            IReviewViolationRepository violationRepository,
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _reviewRepository = reviewRepository;
            _moderationService = moderationService;
            _violationRepository = violationRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ReviewViewModel>> GetRootReviewsByRoomAsync(int roomId)
        {
            var reviews = await _reviewRepository.GetRootReviewsByRoomIdAsync(roomId);
            return reviews.Select(MapToViewModel);
        }

        public async Task<ReviewViewModel> AddReviewAsync(CreateReviewDto dto, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new ArgumentException("Người dùng không tồn tại.");

            // 1. Kiểm tra xem người dùng có đang bị chặn bình luận không
            if (user.ReviewBlockedUntil.HasValue && user.ReviewBlockedUntil > DateTime.UtcNow)
            {
                var localTime = user.ReviewBlockedUntil.Value.ToLocalTime();
                throw new ArgumentException($"Bạn đã vi phạm tiêu chuẩn cộng đồng nhiều lần. Chức năng bình luận của bạn đã bị tạm khóa đến {localTime:HH:mm dd/MM/yyyy}.");
            }

            // 2. Kiểm tra tính hợp lệ của bình luận
            var isAppropriate = await _moderationService.IsCommentAppropriateAsync(dto.Content);
            if (!isAppropriate)
            {
                // Ghi lại vi phạm
                var violation = new ReviewViolation
                {
                    UserId = userId,
                    Content = dto.Content,
                    CreatedAt = DateTime.UtcNow
                };
                await _violationRepository.AddAsync(violation);
                await _violationRepository.SaveChangesAsync();

                // Kiểm tra số lần vi phạm trong thời gian ngắn (ví dụ: 1 giờ)
                var violationCount = await _violationRepository.CountRecentByUserIdAsync(userId, TimeSpan.FromHours(1));

                if (violationCount >= 3)
                {
                    // Chặn 30 phút
                    user.ReviewBlockedUntil = DateTime.UtcNow.AddMinutes(30);
                    await _userManager.UpdateAsync(user);

                    throw new ArgumentException("Bình luận của bạn không phù hợp với tiêu chuẩn cộng đồng của RoomHub. Do vi phạm 3 lần liên tiếp trong thời gian ngắn, chức năng bình luận của bạn đã bị tạm khóa trong 30 phút.");
                }

                throw new ArgumentException($"Bình luận của bạn chứa nội dung không phù hợp với tiêu chuẩn cộng đồng của RoomHub. Vui lòng sử dụng ngôn từ lịch sự hơn. (Số lần vi phạm: {violationCount}/3)");
            }
            var review = new Review
            {
                RoomId = dto.RoomId,
                TenantId = userId, // TenantId is technically the user who wrote it. 
                Comment = dto.Content,
                ParentReviewId = dto.ParentReviewId,
                CreatedAt = DateTime.UtcNow,
                IsModerated = false
            };

            await _reviewRepository.AddAsync(review);
            await _reviewRepository.SaveChangesAsync();

            // Load the relations (Tenant) to return the updated ViewModel.
            // Ideally should have a real mapped tenant returned but we'll fetch it by ID.
            var createdReview = await _reviewRepository.GetByIdAsync(review.Id);
            
            return MapToViewModel(createdReview!);
        }

        private ReviewViewModel MapToViewModel(Review review)
        {
            return new ReviewViewModel
            {
                Id = review.Id,
                Content = review.Comment ?? string.Empty,
                CreatedAt = review.CreatedAt,
                UserId = review.TenantId,
                UserFullName = review.Tenant?.FullName ?? "Unknown User",
                UserAvatarUrl = review.Tenant?.AvatarUrl,
                Replies = review.Replies?.Select(MapToViewModel).OrderBy(r => r.CreatedAt).ToList() ?? new List<ReviewViewModel>()
            };
        }
    }
}
