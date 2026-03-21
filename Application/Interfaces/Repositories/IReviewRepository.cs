using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<Review?> GetByIdAsync(int id);
        Task<IEnumerable<Review>> GetByRoomIdAsync(int roomId);
        Task<IEnumerable<Review>> GetRootReviewsByRoomIdAsync(int roomId);
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(Review review);
        Task SaveChangesAsync();
    }
}
