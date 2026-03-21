using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetConversationAsync(string userId1, string userId2)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                            (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
        public async Task<List<ApplicationUser>> GetContactsAsync(string currentUserId)
        {
            // 1. Lấy ID của những người mà MÌNH ĐÃ GỬI tin nhắn (Truy vấn siêu tốc)
            var sentToIds = await _context.Messages
                .AsNoTracking()
                .Where(m => m.SenderId == currentUserId)
                .Select(m => m.ReceiverId)
                .Distinct()
                .ToListAsync();

            // 2. Lấy ID của những người ĐÃ GỬI CHO MÌNH (Truy vấn siêu tốc)
            var receivedFromIds = await _context.Messages
                .AsNoTracking()
                .Where(m => m.ReceiverId == currentUserId)
                .Select(m => m.SenderId)
                .Distinct()
                .ToListAsync();

            // 3. Gộp 2 danh sách lại và loại bỏ ID trùng lặp trên RAM (cực kỳ nhẹ)
            var contactIds = sentToIds.Union(receivedFromIds).Distinct().ToList();

            if (!contactIds.Any())
            {
                return new List<ApplicationUser>();
            }

            // 4. Lấy ra thông tin User
            return await _context.Users
                .AsNoTracking()
                .Where(u => contactIds.Contains(u.Id))
                .ToListAsync();
        }
    }
}