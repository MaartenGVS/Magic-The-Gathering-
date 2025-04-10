namespace Howest.MagicCards.DAL.Models
{
    public partial class DeckEntry
    {
        public string EntryId { get; init; } = $"deckEntry:{Guid.NewGuid()}";
        public DeckCard Card { get; set; }
        public int Quantity { get; set; } = 1;

    }
}
