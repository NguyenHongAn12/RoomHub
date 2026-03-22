using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Web.Hubs
{
    [Authorize] // Chỉ cho phép user đã đăng nhập kết nối Hub
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(string receiverId, string message)
        {
            // Lấy ID thật từ Cookie/Token của kết nối SignalR (Bảo mật 100%)
            string senderId = Context.UserIdentifier;

            if (string.IsNullOrEmpty(senderId) || string.IsNullOrEmpty(receiverId)) return;

            try
            {
                // Lưu DB
                await _messageService.SaveMessageAsync(senderId, receiverId, message);

                var now = DateTime.UtcNow.ToString("HH:mm");

                // Gửi tin nhắn cho chính người gửi (để hiển thị lên màn hình của họ)
                await Clients.User(senderId).SendAsync("ReceiveMessage", senderId, message, now);

                // Gửi đích danh tin nhắn tới thiết bị của người nhận (Bảo mật)
                await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message, now);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Lỗi Chat]: {ex.Message}");
            }
        }
    }
}