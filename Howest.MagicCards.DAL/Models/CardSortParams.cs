namespace Howest.MagicCards.DAL.Models
{
    public record CardSortParams
    {
        public string OrderBy { get; init; }
        public string OrderDirection { get; set; }

        public CardSortParams(string orderBy, string orderDirection)
        {
            OrderBy = orderBy;
            OrderDirection = orderDirection;
        }
    }

}
