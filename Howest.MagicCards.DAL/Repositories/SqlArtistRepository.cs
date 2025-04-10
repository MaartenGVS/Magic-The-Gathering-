using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlArtistRepository : IArtistRepository
    {

        private readonly CardsContext _db;

        public SqlArtistRepository(CardsContext mtgv1context)
        {
            _db = mtgv1context;
        }

        public async Task<IQueryable<Artist>> GetAllArtistsAsync()
        {
            return await Task.FromResult(_db.Artists);
        }

        public async Task<Artist> GetArtistByIdAsync(long id)
        {
            return await _db.Artists
                                .Include(a => a.Cards)
                                .Where(a => a.Id == id)
                                .FirstOrDefaultAsync();
        }
    }
}
