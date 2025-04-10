namespace Howest.MagicCards.Shared.DTO.CardDT0
{
    public class CardReadDetailDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string SetName { get; set; }
        public string ArtistFullName { get; set; }
        public string Rarity { get; set; }
        public IEnumerable<string> TypeNames { get; set; }
        public string ImageUrl { get; set; }
        public string Text { get; set; }
        public string ManaCost { get; set; }
        public string ConvertedManaCost { get; set; }
        public string RarityCode { get; set; }
        public string SetCode { get; set; }
        public string Flavor { get; set; }
        public long? ArtistId { get; set; }
        public string Number { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public string Layout { get; set; }
        public int? MultiverseId { get; set; }
        public string OriginalText { get; set; }
        public string OriginalType { get; set; }
    }
}
