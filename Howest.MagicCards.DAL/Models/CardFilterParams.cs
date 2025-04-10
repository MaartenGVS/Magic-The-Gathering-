namespace Howest.MagicCards.DAL.Models
{
    public record CardFilterParams(
        string Set,
        string CardType,
        string Artist,
        string Rarity,
        string CardName,
        string CardText
    );
}
