using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        Task AddAsync(Message message);
        Task SaveChangesAsync();
        Task<List<Message>> GetConversationAsync(string userId1, string userId2);
    }
}