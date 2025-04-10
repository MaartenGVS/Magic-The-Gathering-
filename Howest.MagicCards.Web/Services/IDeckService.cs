using Howest.MagicCards.Shared.DTO.DeckDTO;

namespace Howest.MagicCards.Web.Services
{
    public interface IDeckService
    {
        Task<IEnumerable<DeckEntryReadDTO>> GetDeckEntriesAsync();
        Task AddCardToDeckAsync(DeckEntryWriteDTO deckCardRequest);
        Task IncrementDeckEntryAsync(long cardId);
        Task DecrementDeckEntryAsync(long cardId);
        Task ClearDeckAsync();
    }
}
