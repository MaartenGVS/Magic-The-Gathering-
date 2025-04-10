using System.Text.Json;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> DeserializeResponse<T>(this HttpResponseMessage response, JsonSerializerOptions jsonOptions)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<T>(apiResponse, jsonOptions);
            }
            else
            {
                return default;
            }
        }
    }
}