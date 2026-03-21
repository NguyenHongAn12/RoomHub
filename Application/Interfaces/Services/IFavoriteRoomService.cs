using Application.DTOs.RoomPosts;


namespace Application.Interfaces.Services
{
    public interface IFavoriteRoomService
    {

        Task<bool> ToggleFavoriteAsync(string userId, int roomId);

        Task<List<RoomListViewModel>> GetFavoriteRoomsAsync(string userId);
    }
}