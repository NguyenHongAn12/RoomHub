using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IMessageService
    {
        // Hàm lưu tin nhắn mới
        Task<Message> SaveMessageAsync(string senderId, string receiverId, string content);

        // Hàm lấy lịch sử chat giữa 2 người
        Task<List<Message>> GetConversationAsync(string userId1, string userId2);
    }
}