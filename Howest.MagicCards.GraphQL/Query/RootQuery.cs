using GraphQL;
using GraphQL.Types;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.GraphQL.Types;
using CardType = Howest.MagicCards.GraphQL.Types.CardType;

namespace Howest.MagicCards.GraphQL.Query
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery(ICardRepository cardRepository, IArtistRepository artistRepository)
        {
            Name = "Query";

            FieldAsync<ArtistType>(
                "Artist",
                "Get artist by id",
                new QueryArguments
                {
                    new QueryArgument<IdGraphType> { Name = "id", Description = "The unique identifier of the artist." }
                },
                resolve: async context =>
                {
                    long id = context.GetArgument<long>("id");
                    return await artistRepository.GetArtistByIdAsync(id);
                });

            FieldAsync<ListGraphType<ArtistType>>(
                 "Artists",
                 "Get all artists",
                 new QueryArguments
                 {
                new QueryArgument<IntGraphType> { Name = "limit", Description = "The maximum number of artists to return.", DefaultValue = 150 },
                 },
                 resolve: async context =>
                 {
                     int limit = context.GetArgument<int>("limit");
                     IQueryable<Artist> artists = await artistRepository.GetAllArtistsAsync();
                     return artists.Take(limit);
                 });


            FieldAsync<ListGraphType<CardType>>(
             "Cards",
             "Get all cards",
             new QueryArguments
             {
                   new QueryArgument<IntGraphType> { Name = "limit", Description = "The maximum number of cards to return.", DefaultValue = 150 },
                   new QueryArgument<IntGraphType> { Name = "power", Description = "The power of the card."},
                   new QueryArgument<IntGraphType> { Name = "toughness", Description = "The toughness of the card."},
             },
             resolve: async context =>
             {
                 int limit = context.GetArgument<int>("limit");
                 int power = context.GetArgument<int>("power");
                 int toughness = context.GetArgument<int>("toughness");

                 IQueryable<Card> cards = await cardRepository.GetAllCardsAsync();
                 if (power > 0)
                 {
                     cards = cards.Where(c => c.Power == power.ToString());
                 }
                 if (toughness > 0)
                 {
                     cards = cards.Where(c => c.Toughness == toughness.ToString());
                 }
                 return cards.Take(limit);
             });
        }
    }
}
