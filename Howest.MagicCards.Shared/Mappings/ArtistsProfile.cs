using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO.ArtistDTO;

namespace Howest.MagicCards.Shared.Mappings
{
    public class ArtistsProfile : Profile
    {
        public ArtistsProfile()
        {
            CreateMap<Artist, ArtistReadDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.FullName, opt => opt.MapFrom(src => src.FullName));
        }
    }
}
