using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Tenant)
                .Include(r => r.Replies)
                    .ThenInclude(reply => reply.Tenant)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Review>> GetByRoomIdAsync(int roomId)
        {
            return await _context.Reviews
                .Include(r => r.Tenant)
                .Where(r => r.RoomId == roomId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetRootReviewsByRoomIdAsync(int roomId)
        {
            return await _context.Reviews
                .Include(r => r.Tenant)
                .Include(r => r.Replies) // Only load first-level replies
                    .ThenInclude(reply => reply.Tenant)
                .Where(r => r.RoomId == roomId && r.ParentReviewId == null)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Review review)
        {
            _context.Reviews.Remove(review);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
