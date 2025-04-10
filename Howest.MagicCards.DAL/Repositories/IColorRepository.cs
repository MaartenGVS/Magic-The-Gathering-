using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IColorRepository
    {
        Task<IEnumerable<Color>> GetAllColorsAsync();
        Task<Color> GetColorByIdAsync(int id);
    }
}
