
using Type = Howest.MagicCards.DAL.Models.Type;


namespace Howest.MagicCards.DAL.Repositories
{
    public interface ITypeRepository
    {
        Task<IEnumerable<Type>> GetAllTypesAsync();
        Task<Type> GetTypeByIdAsync(int id);
    }
}
