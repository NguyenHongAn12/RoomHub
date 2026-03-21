using Application.DTOs.Reviews;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] CreateReviewDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = GetUserId();
                var newReview = await _reviewService.AddReviewAsync(dto, userId);
                return Ok(newReview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the review.", details = ex.Message });
            }
        }
    }
}
