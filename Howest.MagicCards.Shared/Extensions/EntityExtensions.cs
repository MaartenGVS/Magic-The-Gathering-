using Howest.MagicCards.DAL.Models;

public static class EntityExtensions
{
    public static IQueryable<T> ToPagedList<T>(this IQueryable<T> entities, int pageNumber, int pageSize)
    {
        return entities
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
    }

    public static IQueryable<Card> Sort(this IQueryable<Card> cards, string orderBy, string orderDirection)
    {
        if (string.IsNullOrEmpty(orderBy) || string.IsNullOrEmpty(orderDirection))
        {
            return cards; 
        }

        switch (orderBy.ToLower())
        {
            case "cardname":
                return orderDirection?.ToLower() == "desc"
                    ? cards.OrderByDescending(c => c.Name)
                    : cards.OrderBy(c => c.Name);
            case "artistname":
                return orderDirection?.ToLower() == "desc"
                    ? cards.OrderByDescending(c => c.Artist.FullName)
                    : cards.OrderBy(c => c.Artist.FullName);
            case "setname":
                    return orderDirection?.ToLower() == "desc"
                    ? cards.OrderByDescending(c => c.Set.Name)
                    : cards.OrderBy(c => c.Set.Name);
            case "rarityname":
                return orderDirection?.ToLower() == "desc"
                    ? cards.OrderByDescending(c => c.Rarity.Name)
                    : cards.OrderBy(c => c.Rarity.Name);
            default:
                return cards; 
        }
    }
}