using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BlogApp.Services
{
    public class DeepLTranslationService : ITranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public DeepLTranslationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["DeepL:ApiKey"];
        }

        public async Task<string> TranslateAsync(string text, string targetLang)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api-free.deepl.com/v2/translate");
            request.Headers.Add("Authorization", $"DeepL-Auth-Key {_apiKey}");

            var body = new
            {
                text = new[] { text },
                target_lang = targetLang
            };

            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement
                .GetProperty("translations")[0]
                .GetProperty("text")
                .GetString();
        }
    }
}