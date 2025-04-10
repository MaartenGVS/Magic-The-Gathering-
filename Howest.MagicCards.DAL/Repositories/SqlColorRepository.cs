using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlColorRepository : IColorRepository
    {

        private readonly CardsContext _db;

        public SqlColorRepository(CardsContext mtgv1context)
        {
            _db = mtgv1context;
        }

      public async Task<IEnumerable<Color>> GetAllColorsAsync()
        {
            return await Task.FromResult(_db.Colors);
        }

        public async Task<Color> GetColorByIdAsync(int id)
        {
            return await _db.Colors
                                .Where(c => c.Id == id)
                                .FirstOrDefaultAsync();
        }
    }
}
