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

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<ReviewViewModel>> GetRootReviewsByRoomAsync(int roomId)
        {
            var reviews = await _reviewRepository.GetRootReviewsByRoomIdAsync(roomId);
            return reviews.Select(MapToViewModel);
        }

        public async Task<ReviewViewModel> AddReviewAsync(CreateReviewDto dto, string userId)
        {
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
