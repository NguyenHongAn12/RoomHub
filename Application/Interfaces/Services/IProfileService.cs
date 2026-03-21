using Application.DTOs.Profile;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IProfileService
    {
        Task<ProfileDto> GetUserProfileAsync(string userId);
        Task<bool> UpdateUserProfileAsync(UpdateProfileDto updateDto);
    }
}
