using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReviewViolationRepository : IReviewViolationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewViolationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ReviewViolation violation)
        {
            await _context.ReviewViolations.AddAsync(violation);
        }

        public async Task<int> CountRecentByUserIdAsync(string userId, TimeSpan window)
        {
            var cutoff = DateTime.UtcNow.Subtract(window);
            return await _context.ReviewViolations
                .CountAsync(v => v.UserId == userId && v.CreatedAt >= cutoff);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
