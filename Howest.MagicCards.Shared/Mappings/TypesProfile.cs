using AutoMapper;
using Howest.MagicCards.Shared.DTO.TypeDTO;
using Type = Howest.MagicCards.DAL.Models.Type;

namespace Howest.MagicCards.Shared.Mappings
{
    public class TypesProfile : Profile
    {
        public TypesProfile()
        {
            CreateMap<Type, TypeReadDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
