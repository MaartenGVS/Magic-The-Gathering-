namespace Howest.MagicCards.Shared.DTO.DeckDTO
{
    public record DeckCardReadDTO
    {
        public long Id { get; init; }
        public string Name { get; init; }
        public string ManaCost { get; init; }
    }
}
