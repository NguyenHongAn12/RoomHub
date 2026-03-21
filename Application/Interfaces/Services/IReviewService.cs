using Application.DTOs.Reviews;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewViewModel>> GetRootReviewsByRoomAsync(int roomId);
        Task<ReviewViewModel> AddReviewAsync(CreateReviewDto dto, string userId);
    }
}
