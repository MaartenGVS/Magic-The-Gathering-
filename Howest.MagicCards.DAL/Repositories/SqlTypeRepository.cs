using Howest.MagicCards.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Type = Howest.MagicCards.DAL.Models.Type;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlTypeRepository : ITypeRepository
    {
        private readonly CardsContext _db;

        public SqlTypeRepository(CardsContext mtgv1context)
        {
            _db = mtgv1context;
        }

        public async Task<IEnumerable<Type>> GetAllTypesAsync()
        {
            return await Task.FromResult(_db.Types);
        }

        public async Task<Type> GetTypeByIdAsync(int id)
        {
            return await _db.Types
                               .Where(a => a.Id == id)
                               .FirstOrDefaultAsync();
        }
    }
}
