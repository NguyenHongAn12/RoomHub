using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ICloudinaryService
    {
        Task<(string Url, string PublicId)> UploadImageAsync(IFormFile file, string folder = "roomhub");
        Task DeleteImageAsync(string publicId);
    }
}
