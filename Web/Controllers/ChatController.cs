using Application.Interfaces;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly IMessageService _messageService;

        public ChatController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<IActionResult> Tenant()
        {
            // Lấy lịch sử chat của 2 ID ảo này truyền ra View
            var history = await _messageService.GetConversationAsync("tenant-123", "owner-456");
            return View(history);
        }

        public async Task<IActionResult> Owner()
        {
            var history = await _messageService.GetConversationAsync("tenant-123", "owner-456");
            return View(history);
        }
    }
}