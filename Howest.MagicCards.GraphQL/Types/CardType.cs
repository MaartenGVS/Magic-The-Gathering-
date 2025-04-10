using GraphQL.Types;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;

namespace Howest.MagicCards.GraphQL.Types
{
    public class CardType : ObjectGraphType<Card>
    {
        public CardType(IArtistRepository artistRepository)
        {
            Name = "Card";
            Field(c => c.Id, type: typeof(IdGraphType)).Description("The unique identifier of the card.").Name("Id");
            Field(c => c.Name, type: typeof(StringGraphType)).Description("The name of the card.").Name("Name");
            Field(c => c.Text, type: typeof(StringGraphType)).Description("The Text of the card.").Name("Text");
            Field(c => c.Type, type: typeof(StringGraphType)).Description("The Type of the card.").Name("Type");
            Field(c => c.RarityCode, type: typeof(StringGraphType)).Description("The RarityCode of the card.").Name("RarityCode");
            Field(c => c.SetCode, type: typeof(StringGraphType)).Description("The SetCode of the card.").Name("SetCode");
            Field(c => c.Flavor, type: typeof(StringGraphType)).Description("The Flavor of the card.").Name("Flavor");
            Field(c => c.ManaCost, type: typeof(StringGraphType)).Description("The ManaCost of the card.").Name("ManaCost");
            Field(c => c.Power, type: typeof(StringGraphType)).Description("The Power of the card.").Name("Power");
            Field(c => c.Toughness, type: typeof(StringGraphType)).Description("The Toughness of the card.").Name("Toughness");
            Field(c => c.Artist, type: typeof(ArtistType));
        }
    }
}
