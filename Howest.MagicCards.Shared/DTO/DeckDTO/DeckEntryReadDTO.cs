namespace Howest.MagicCards.Shared.DTO.DeckDTO
{
    public record DeckEntryReadDTO
    {
        public string EntryId { get; init; }

        public DeckCardReadDTO Card { get; init; }

        public int Quantity { get; init; }
    }
}
