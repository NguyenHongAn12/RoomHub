using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRoomPostRepository
    {
        Task<IEnumerable<Room>> GetAllActiveAsync();
        Task<IEnumerable<Room>> GetByLandlordIdAsync(string landlordId);
        Task<Room?> GetByIdAsync(int id);
        Task AddAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(Room room);

        // Lookup data helpers
        Task<IEnumerable<Floor>> GetFloorsByLandlordIdAsync(string landlordId);
        Task<IEnumerable<Amenity>> GetAllAmenitiesAsync();
    }
}