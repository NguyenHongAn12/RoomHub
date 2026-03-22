using Application.DTOs.RoomPosts;

namespace Application.Interfaces.Services
{
    public interface IRoomPostService
    {
        // Thêm optional params - các dev gọi GetAllRoomsAsync() vẫn hoạt động bình thường
        Task<IEnumerable<RoomListViewModel>> GetAllRoomsAsync(string? keyword = null, string? province = null, Domain.Enums.RoomType? roomType = null);

        // Search suggestions for autocomplete dropdown
        Task<IEnumerable<RoomSuggestionDto>> GetSuggestionsAsync(string keyword, string? province = null, int maxResults = 6);
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