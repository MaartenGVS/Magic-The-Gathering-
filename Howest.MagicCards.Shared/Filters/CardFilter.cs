namespace Howest.MagicCards.Shared.Filters
{
    public class CardFilter : PaginationFilter
    {
        public string SetName { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public string RarityName { get; set; } = string.Empty;
        public string CardTypeName { get; set; } = string.Empty;
        public string CardName { get; set; } = string.Empty;
        public string CardText { get; set; } = string.Empty;
    }
}
