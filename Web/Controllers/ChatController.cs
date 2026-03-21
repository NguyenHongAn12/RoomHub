using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly UserManager<ApplicationUser> _userManager;

        // Bơm thêm UserManager vào Constructor
        public ChatController(IMessageService messageService, UserManager<ApplicationUser> userManager)
        {
            _messageService = messageService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string receiverId = null)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var history = new List<Message>();
            string receiverName = "Người dùng ẩn danh";

            // 1. Lấy danh sách những người đã từng chat
            var contacts = await _messageService.GetContactsAsync(currentUserId);

            // 2. Nếu vào trang Chat từ Header (không có receiverId), tự động chọn người đầu tiên
            if (string.IsNullOrEmpty(receiverId) && contacts.Any())
            {
                receiverId = contacts.First().Id;
            }

            // 3. Nếu xác định được người cần chat
            if (!string.IsNullOrEmpty(receiverId))
            {
                // Lấy lịch sử tin nhắn
                history = await _messageService.GetConversationAsync(currentUserId, receiverId);
                ViewBag.CurrentReceiverId = receiverId;

                // Lấy tên hiển thị của người nhận
                var receiverUser = await _userManager.FindByIdAsync(receiverId);
                if (receiverUser != null)
                {
                    receiverName = receiverUser.FullName ?? receiverUser.UserName;
                }
            }

            // Truyền dữ liệu ra View
            ViewBag.CurrentUserId = currentUserId;
            ViewBag.Contacts = contacts;
            ViewBag.CurrentReceiverName = receiverName; // Truyền tên thật ra View

            return View(history);
        }
    }
}