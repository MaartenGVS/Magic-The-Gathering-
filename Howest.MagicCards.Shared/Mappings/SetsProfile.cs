using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO.ArtistDTO;
using Howest.MagicCards.Shared.DTO.SetDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Mappings
{
    public class SetsProfile: Profile
    {
        public SetsProfile()
        {
            CreateMap<Set, SetReadDTO>()
               .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dto => dto.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
