using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RoomHub.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class FavoriteRoomRepository : IFavoriteRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public FavoriteRoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FavoriteRoom favoriteRoom)
        {
            await _context.FavoriteRooms.AddAsync(favoriteRoom);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(FavoriteRoom favoriteRoom)
        {
            _context.FavoriteRooms.Remove(favoriteRoom);
            await _context.SaveChangesAsync();
        }

        public async Task<FavoriteRoom?> GetAsync(string userId, int roomId)
        {
            return await _context.FavoriteRooms
                .FirstOrDefaultAsync(f => f.UserId == userId && f.RoomId == roomId);
        }

        public async Task<IEnumerable<FavoriteRoom>> GetByUserIdAsync(string userId)
        {
            return await _context.FavoriteRooms
                .Include(f => f.Room)
                    .ThenInclude(r => r.RoomPhotos)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<int>> GetFavoriteRoomIdsAsync(string userId)
        {
            return await _context.FavoriteRooms
                .Where(f => f.UserId == userId)
                .Select(f => f.RoomId)
                .ToListAsync();
        }
    }
}