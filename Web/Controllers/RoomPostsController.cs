using Application.DTOs.RoomPosts;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize(Roles = "PropertyOwner")]
    public class RoomPostsController : Controller
    {
        private readonly IRoomPostService _roomPostService;
        private readonly IReviewService _reviewService;

        public RoomPostsController(IRoomPostService roomPostService, IReviewService reviewService)
        {
            _roomPostService = roomPostService;
            _reviewService = reviewService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();
            //return "test-user-id-123";
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? q, string? province, Domain.Enums.RoomType? roomType)
        {
            string? currentUserId = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var rooms = await _roomPostService.GetAllRoomsAsync(currentUserId);
            return View(rooms);
        }

        public async Task<IActionResult> MyPosts()
        {
            var userId = GetUserId();
            var rooms = await _roomPostService.GetMyRoomsAsync(userId);
            return View(rooms);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var viewModel = await _roomPostService.GetRoomDetailsAsync(id);
                var reviews = await _reviewService.GetRootReviewsByRoomAsync(id);
                viewModel.Reviews = reviews.ToList();
                return View(viewModel);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SearchSuggestions(string q, string? province)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
                return Json(new List<object>());

            var suggestions = await _roomPostService.GetSuggestionsAsync(q.Trim(), province, 6);
            return Json(suggestions);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = GetUserId();
            var viewModel = await _roomPostService.GetCreateViewModelAsync(userId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoomViewModel model)
        {
            var userId = GetUserId();
            
            if (model.IsNewBuilding)
            {
                ModelState.Remove(nameof(model.FloorId));
            }

            if (!ModelState.IsValid)
            {
                var viewModel = await _roomPostService.GetCreateViewModelAsync(userId);
                model.AvailableFloors = viewModel.AvailableFloors;
                model.AvailableAmenities = viewModel.AvailableAmenities;
                return View(model);
            }

            try
            {
                await _roomPostService.CreateRoomAsync(model, userId);
                TempData["Success"] = "Room post created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating room post: " + ex.Message;
                var viewModel = await _roomPostService.GetCreateViewModelAsync(userId);
                model.AvailableFloors = viewModel.AvailableFloors;
                model.AvailableAmenities = viewModel.AvailableAmenities;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetUserId();
            try
            {
                var viewModel = await _roomPostService.GetEditViewModelAsync(id, userId);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditRoomViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var userId = GetUserId();

            if (!ModelState.IsValid)
            {
                var viewModel = await _roomPostService.GetEditViewModelAsync(id, userId);
                model.AvailableFloors = viewModel.AvailableFloors;
                model.AvailableAmenities = viewModel.AvailableAmenities;
                return View(model);
            }

            try
            {
                await _roomPostService.UpdateRoomAsync(model, userId);
                TempData["Success"] = "Room post updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating room post: " + ex.Message;
                var viewModel = await _roomPostService.GetEditViewModelAsync(id, userId);
                model.AvailableFloors = viewModel.AvailableFloors;
                model.AvailableAmenities = viewModel.AvailableAmenities;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            try
            {
                await _roomPostService.DeleteRoomAsync(id, userId);
                TempData["Success"] = "Room post deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting room post: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
