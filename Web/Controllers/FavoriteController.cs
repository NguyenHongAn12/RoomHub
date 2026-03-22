using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize] 
    public class FavoriteController : Controller
    {
        private readonly IFavoriteRoomService _favoriteService;

        public FavoriteController(IFavoriteRoomService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await _favoriteService.GetFavoriteRoomsAsync(userId);
            return View("~/Views/Favorite/Index.cshtml", favorites);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int roomId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "Vui lòng đăng nhập" });
            }

            try
            {
                var isFavorite = await _favoriteService.ToggleFavoriteAsync(userId, roomId);
                return Json(new { success = true, isFavorite });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}