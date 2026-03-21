using Application.Interfaces;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // Nhận SenderId và ReceiverId trực tiếp từ Giao diện
        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            // SAU NÀY KHI CÓ LOGIN: Bạn có thể lấy ID người gửi bằng lệnh sau
            // string realSenderId = Context.UserIdentifier;

            try
            {
                // Cố gắng lưu vào Database
                await _messageService.SaveMessageAsync(senderId, receiverId, message);
            }
            catch (Exception ex)
            {
                // Nếu User chưa tồn tại trong DB (do team Auth chưa làm xong), 
                // in ra log chứ không làm sập chức năng Chat.
                Console.WriteLine($"[Cảnh báo Chat] Chưa lưu được DB vì: {ex.InnerException?.Message ?? ex.Message}");
            }

            // Phát sóng tin nhắn lại cho TẤT CẢ màn hình.
            await Clients.All.SendAsync("ReceiveMessage", senderId, message);
        }
    }
}