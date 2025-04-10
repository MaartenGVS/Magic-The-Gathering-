using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO.ArtistDTO;
using Howest.MagicCards.Shared.DTO.CardDT0;
using Howest.MagicCards.Shared.DTO.SetDTO;
using Howest.MagicCards.Shared.DTO.TypeDTO;
using Howest.MagicCards.Shared.Extensions;
using Howest.MagicCards.WebAPI.Wrappers;
using System.Text.Json;

namespace Howest.MagicCards.Web.Services
{
    public class CardService : ICardService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public CardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IEnumerable<ArtistReadDTO>> GetAllArtistsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Artists");
            return await response.DeserializeResponse<IEnumerable<ArtistReadDTO>>(_jsonOptions);
        }

        public async Task<IEnumerable<TypeReadDTO>> GetAllTypesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Types");
            return await response.DeserializeResponse<IEnumerable<TypeReadDTO>>(_jsonOptions);
        }

        public async Task<IEnumerable<SetReadDTO>> GetAllSetsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Sets");
            return await response.DeserializeResponse<IEnumerable<SetReadDTO>>(_jsonOptions);
        }

        public async Task<PagedResponse<IEnumerable<CardReadDetailDTO>>> GetAllCardsAsync(CardFilterParams filterParams, CardSortParams sortParams, int currentPage, int pageSize)
        {
            string queryString = $"SetName={filterParams?.Set ?? string.Empty}&" +
                                 $"ArtistName={filterParams?.Artist ?? string.Empty}&" +
                                 $"RarityName={filterParams?.Rarity ?? string.Empty}&" +
                                 $"CardTypeName={filterParams?.CardType ?? string.Empty}&" +
                                 $"CardName={filterParams?.CardName ?? string.Empty}&" +
                                 $"CardText={filterParams?.CardText ?? string.Empty}&" +
                                 $"orderBy={sortParams.OrderBy}&" +
                                 $"orderDirection={sortParams.OrderDirection ?? string.Empty}&" +
                                 $"pageNumber={currentPage}&" +
                                 $"pageSize={pageSize}";

            HttpResponseMessage response = await _httpClient.GetAsync($"Cards?{queryString}");
            return await response.DeserializeResponse<PagedResponse<IEnumerable<CardReadDetailDTO>>>(_jsonOptions);
        }


    }
}