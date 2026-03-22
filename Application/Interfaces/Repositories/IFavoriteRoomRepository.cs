using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoomHub.Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IFavoriteRoomRepository
    {
        Task AddAsync(FavoriteRoom favoriteRoom);
        Task RemoveAsync(FavoriteRoom favoriteRoom);
        Task<FavoriteRoom?> GetAsync(string userId, int roomId);
        Task<IEnumerable<FavoriteRoom>> GetByUserIdAsync(string userId);
        Task<List<int>> GetFavoriteRoomIdsAsync(string userId);
    }
}