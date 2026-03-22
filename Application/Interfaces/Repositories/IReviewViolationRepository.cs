using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IReviewViolationRepository
    {
        Task AddAsync(ReviewViolation violation);
        Task<int> CountRecentByUserIdAsync(string userId, TimeSpan window);
        Task SaveChangesAsync();
    }
}
