using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IGeminiModerationService
    {
        Task<bool> IsCommentAppropriateAsync(string comment);
    }
}
