using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO.ArtistDTO;
using Howest.MagicCards.Shared.DTO.CardDT0;
using Howest.MagicCards.Shared.DTO.SetDTO;
using Howest.MagicCards.Shared.DTO.TypeDTO;
using Howest.MagicCards.WebAPI.Wrappers;

namespace Howest.MagicCards.Web.Services
{
    public interface ICardService
    {
        Task<IEnumerable<ArtistReadDTO>> GetAllArtistsAsync();
        Task<IEnumerable<TypeReadDTO>> GetAllTypesAsync();
        Task<IEnumerable<SetReadDTO>> GetAllSetsAsync();
        Task<PagedResponse<IEnumerable<CardReadDetailDTO>>> GetAllCardsAsync(CardFilterParams filterParams, CardSortParams sortParams, int currentPage, int pageSize);
    }
}
