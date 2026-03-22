using Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GeminiModerationService : IGeminiModerationService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;

        public GeminiModerationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GeminiApiKey"];
        }

        public async Task<bool> IsCommentAppropriateAsync(string comment)
        {
            if (string.IsNullOrWhiteSpace(_apiKey) || string.IsNullOrWhiteSpace(comment))
            {
                return true; // Optionally default to true if no key or comment
            }

            try
            {
                var modelName = "gemini-2.5-flash"; 
                var requestUrl = $"https://generativelanguage.googleapis.com/v1/models/{modelName}:generateContent?key={_apiKey}";
                
                var prompt = $"Bạn là một AI kiểm duyệt nội dung của một nền tảng thuê phòng. Hãy kiểm tra phần đánh giá sau có chứa ngôn từ thô tục, chửi thề, xúc phạm hay phản cảm không. Hãy chỉ trả lời duy nhất bằng một từ 'APPROVE' nếu hợp lệ, và 'REJECT' nếu vi phạm.\nBình luận: {comment}";

                var payload = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(requestUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // Trả về false (REJECT) nếu API lỗi để đảm bảo an toàn, hoặc throw để debug
                    throw new System.Exception($"Moderation API Error: {response.StatusCode} - {responseString}");
                }

                using var jsonDoc = JsonDocument.Parse(responseString);
                
                var root = jsonDoc.RootElement;
                if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                {
                    var firstCandidate = candidates[0];
                    
                    // Kiểm tra xem Gemini có chặn nội dung này vì lý do an toàn không (thường là do quá thô tục)
                    if (firstCandidate.TryGetProperty("finishReason", out var reason) && 
                        (reason.GetString() == "SAFETY" || reason.GetString() == "OTHER"))
                    {
                        return false; // Bị chặn bởi bộ lọc của Google -> REJECT
                    }

                    if (firstCandidate.TryGetProperty("content", out var contentElement))
                    {
                        if (contentElement.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                        {
                            var firstPart = parts[0];
                            if (firstPart.TryGetProperty("text", out var textElement))
                            {
                                var text = textElement.GetString()?.Trim().ToUpperInvariant() ?? "APPROVE";
                                // Kiểm tra xem phản hồi có chứa từ REJECT không (linh hoạt hơn việc so khớp tuyệt đối)
                                return !text.Contains("REJECT");
                            }
                        }
                    }
                }

                // Nếu không có kết quả rõ ràng, kiểm tra xem có promptFeedback không (thường bị chặn do an toàn)
                if (root.TryGetProperty("promptFeedback", out var feedback))
                {
                    if (feedback.TryGetProperty("blockReason", out var blockReason))
                    {
                        return false; // Bị chặn ngay từ đầu -> REJECT
                    }
                }

                return true;
            }
            catch
            {
                return true;
            }
        }
    }
}
