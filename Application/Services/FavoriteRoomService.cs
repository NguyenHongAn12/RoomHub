using Application.DTOs.RoomPosts;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using RoomHub.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FavoriteRoomService : IFavoriteRoomService
    {
        private readonly IFavoriteRoomRepository _favoriteRepo;

        public FavoriteRoomService(IFavoriteRoomRepository favoriteRepo)
        {
            _favoriteRepo = favoriteRepo;
        }

        public async Task<bool> ToggleFavoriteAsync(string userId, int roomId)
        {
            var existingFavorite = await _favoriteRepo.GetAsync(userId, roomId);

            if (existingFavorite != null)
            {
                await _favoriteRepo.RemoveAsync(existingFavorite);
                return false;
            }
            else
            {
                var newFavorite = new FavoriteRoom
                {
                    UserId = userId,
                    RoomId = roomId
                };
                await _favoriteRepo.AddAsync(newFavorite);
                return true;
            }
        }

        public async Task<List<RoomListViewModel>> GetFavoriteRoomsAsync(string userId)
        {
            var favorites = await _favoriteRepo.GetByUserIdAsync(userId);

            return favorites.Select(f => new RoomListViewModel
            {
                Id = f.Room.Id,
                Title = f.Room.Title,
                BasePrice = f.Room.BasePrice,
                SurfaceArea = f.Room.SurfaceArea,
                LandlordId = f.Room.LandlordId ?? string.Empty, 
                IsFavorite = true 
            }).ToList();
        }
    }
}