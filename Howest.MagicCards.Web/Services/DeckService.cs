using Howest.MagicCards.Shared.DTO.DeckDTO;
using Howest.MagicCards.Shared.Extensions;
using System.Text.Json;

namespace Howest.MagicCards.Web.Services
{
    public class DeckService : IDeckService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public DeckService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IEnumerable<DeckEntryReadDTO>> GetDeckEntriesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("deckEntries");
            return await response.DeserializeResponse<IEnumerable<DeckEntryReadDTO>>(_jsonOptions);
        }

        public async Task AddCardToDeckAsync(DeckEntryWriteDTO deckCardRequest)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("deckEntries", deckCardRequest);
            response.EnsureSuccessStatusCode();
        }

        public async Task IncrementDeckEntryAsync(long cardId)
        {
            HttpResponseMessage response = await _httpClient.PatchAsync($"deckEntries/cards/{cardId}/increase", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task DecrementDeckEntryAsync(long cardId)
        {
            HttpResponseMessage response = await _httpClient.PatchAsync($"deckEntries/cards/{cardId}/decrease", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task ClearDeckAsync()
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync("deckEntries");
            response.EnsureSuccessStatusCode();
        }
    }
}