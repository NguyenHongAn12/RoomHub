using Application.Interfaces.Services;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        // Bơm Repository vào thay vì DbContext
        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Message> SaveMessageAsync(string senderId, string receiverId, string content)
        {
            // Ở đây bạn có thể viết thêm Business Logic. Ví dụ:
            // if (string.IsNullOrWhiteSpace(content)) throw new Exception("Tin nhắn rỗng");

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            // Gọi Repository để lưu
            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();

            return message;
        }

        public async Task<List<Message>> GetConversationAsync(string userId1, string userId2)
        {
            return await _messageRepository.GetConversationAsync(userId1, userId2);
        }
    }
}