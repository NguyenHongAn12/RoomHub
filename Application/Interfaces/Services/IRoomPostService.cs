using Application.DTOs.RoomPosts;

namespace Application.Interfaces.Services
{
    public interface IRoomPostService
    {
        Task<IEnumerable<RoomListViewModel>> GetAllRoomsAsync(string? currentUserId = null);
        Task<IEnumerable<RoomListViewModel>> GetMyRoomsAsync(string landlordId);

        // GET lookup data combined with viewmodels
        Task<CreateRoomViewModel> GetCreateViewModelAsync(string landlordId);
        Task<EditRoomViewModel> GetEditViewModelAsync(int id, string landlordId);
        Task<RoomDetailsViewModel> GetRoomDetailsAsync(int id);

        Task CreateRoomAsync(CreateRoomViewModel model, string landlordId);
        Task UpdateRoomAsync(EditRoomViewModel model, string currentUserId);
        Task DeleteRoomAsync(int id, string currentUserId);
    }
}