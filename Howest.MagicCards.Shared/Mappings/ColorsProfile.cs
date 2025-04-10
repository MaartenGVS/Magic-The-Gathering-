using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO.ColorDTO;

namespace Howest.MagicCards.Shared.Mappings
{
    public class ColorsProfile : Profile
    {
        public ColorsProfile()
        {
            CreateMap<Color, ColorReadDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
