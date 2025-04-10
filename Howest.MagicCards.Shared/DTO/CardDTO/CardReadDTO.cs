using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO.CardDT0
{
    public record CardReadDTO
    {
        public long Id { get; init; }
        public string SetName { get; init; }
        public string ArtistFullName { get; init; }
        public string ManaCost { get; set; }
        public string Rarity { get; init; }
        public IEnumerable<string> TypeNames { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }
        public string Text { get; init; }
    }
}
