using GraphQL.Types;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;

namespace Howest.MagicCards.GraphQL.Types
{
    public class ArtistType : ObjectGraphType<Artist>
    {
        public ArtistType(ICardRepository cardRepository)
        {
            Name = "Artist";

            Field(a => a.Id, type: typeof(IdGraphType)).Description("The unique identifier of the artist.").Name("Id");
            Field(a => a.FullName, type: typeof(StringGraphType)).Description("The full name of the artist.").Name("FullName");
            Field(a => a.Cards, type: typeof(ListGraphType<CardType>));
        }
    }
}
