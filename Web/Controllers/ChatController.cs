using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly UserManager<ApplicationUser> _userManager;
        // Bổ sung dịch vụ xử lý ảnh Cloudinary
        private readonly ICloudinaryService _cloudinaryService;

        public ChatController(
            IMessageService messageService,
            UserManager<ApplicationUser> userManager,
            ICloudinaryService cloudinaryService)
        {
            _messageService = messageService;
            _userManager = userManager;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> Index(string receiverId = null)
        {
            // ... (GIỮ NGUYÊN TOÀN BỘ LOGIC CŨ Ở ĐÂY) ...
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var history = new List<Message>();
            string receiverName = "Người dùng ẩn danh";

            var contacts = await _messageService.GetContactsAsync(currentUserId);

            if (string.IsNullOrEmpty(receiverId) && contacts.Any())
            {
                receiverId = contacts.First().Id;
            }

            if (!string.IsNullOrEmpty(receiverId))
            {
                history = await _messageService.GetConversationAsync(currentUserId, receiverId);
                ViewBag.CurrentReceiverId = receiverId;

                var receiverUser = await _userManager.FindByIdAsync(receiverId);
                if (receiverUser != null)
                {
                    ViewBag.CurrentReceiverName = receiverUser.FullName ?? receiverUser.UserName;
                }
            }

            ViewBag.CurrentUserId = currentUserId;
            ViewBag.Contacts = contacts;

            return View(history);
        }

        // ================= API MỚI: XỬ LÝ UPLOAD ẢNH CHAT ================= //
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest(new { success = false, message = "Vui lòng chọn một bức ảnh hợp lệ." });

            try
            {
                // Gọi CloudinaryService của bạn lưu vào thư mục "chat_images"
                var result = await _cloudinaryService.UploadImageAsync(image, "chat_images");

                // Trả về URL an toàn của bức ảnh
                return Json(new { success = true, imageUrl = result.Url });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}