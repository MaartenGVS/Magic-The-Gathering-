
using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlCardRepository : ICardRepository
    {

        private readonly CardsContext _db;

        public SqlCardRepository(CardsContext mtgv1context)
        {
            _db = mtgv1context;
        }

        public async Task<IQueryable<Card>> GetAllCardsAsync()
        {
            return await Task.FromResult(_db.Cards.Include(c =>c.Artist).Select(b => b));           
        }

        public Task<IQueryable<Card>> GetAllCardsByArtistIdAsync(long id)
        {
            return Task.FromResult(_db.Cards
                                                .Where(c => c.ArtistId == id));
        }

        public Task<Card> GetCardByIdAsync(long id)
        {
            return _db.Cards
                        .Include(c => c.Set)
                        .Include(c => c.Artist)
                        .Include(c => c.Rarity)
                        .Include(c => c.CardTypes)
                            .ThenInclude(ct => ct.Type)
                        .Where(c => c.Id == id)
                        .FirstAsync();
        }
    }
}
