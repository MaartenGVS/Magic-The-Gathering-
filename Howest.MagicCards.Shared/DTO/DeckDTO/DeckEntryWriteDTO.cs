using Howest.MagicCards.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Howest.MagicCards.Shared.DTO.DeckDTO
{
    public record DeckEntryWriteDTO
    {
        public string EntryId { get; set; }
        public DeckCardReadDTO Card { get; set; }
        public int Quantity { get; set; }
    }
}