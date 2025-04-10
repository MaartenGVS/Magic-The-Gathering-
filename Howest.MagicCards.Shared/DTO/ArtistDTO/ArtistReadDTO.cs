using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTO.ArtistDTO
{
    public record ArtistReadDTO
    {
        public long Id { get; set; }
        public string FullName { get; set; }
    }
}
