using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlSetRepository : ISetRepository
    {

        private readonly CardsContext _db;

        public SqlSetRepository(CardsContext mtgv1context)
        {
            _db = mtgv1context;
        }

        public async Task<IEnumerable<Set>> GetAllSetsAsync()
        {
            return await Task.FromResult(_db.Sets);
        }

        public async Task<Set> GetSetByIdAsync(int id)
        {
            return await _db.Sets
                               .Where(s => s.Id == id)
                               .FirstOrDefaultAsync();
        }
    }
}
