using Howest.MagicCards.DAL.Models;


namespace Howest.MagicCards.DAL.Repositories
{
    public interface IDeckRepository
    {
        Task<IEnumerable<DeckEntry>> GetDeckEntriesAsync();
        void AddDeckEntryAsync(DeckEntry deckCard);
        Task<DeckEntry> GetDeckCardByCardIdAsync(long cardId);
        void UpdateDeckEntryAsync(DeckEntry deckCard);
        void DeleteDeckEntryAsync(DeckEntry deckCard);
        void DeleteAllEntriesAsync();
        Task<DeckEntry> GetDeckEntryByIdAsync(string entryId);
    }
}
