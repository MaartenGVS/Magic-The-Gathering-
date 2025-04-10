using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface ICardRepository
    {
        Task<IQueryable<Card>> GetAllCardsAsync();
        Task<IQueryable<Card>> GetAllCardsByArtistIdAsync(long id);
        Task<Card> GetCardByIdAsync(long id);
    }
}
