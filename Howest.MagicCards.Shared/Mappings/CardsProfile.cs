using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO.CardDT0;

namespace Howest.MagicCards.Shared.Mappings
{

    public class CardsProfile : Profile
    {
        public CardsProfile()
        {
            CreateMap<Card, CardReadDTO>()
                 .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dto => dto.SetName, opt => opt.MapFrom(src => src.Set.Name))
                 .ForMember(dto => dto.ArtistFullName, opt => opt.MapFrom(src => src.Artist.FullName))
                 .ForMember(dto => dto.Rarity, opt => opt.MapFrom(src => src.Rarity.Name))
                 .ForMember(dto => dto.TypeNames, opt => opt.MapFrom(src => src.CardTypes.Select(ct => ct.Type.Name)))
                 .ForMember(dto => dto.ImageUrl, opt => opt.MapFrom(src => src.OriginalImageUrl));

            CreateMap<Card, CardReadDetailDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.SetName, opt => opt.MapFrom(src => src.Set.Name))
                .ForMember(dto => dto.ArtistFullName, opt => opt.MapFrom(src => src.Artist.FullName))
                .ForMember(dto => dto.Rarity, opt => opt.MapFrom(src => src.Rarity.Name))
                .ForMember(dto => dto.TypeNames, opt => opt.MapFrom(src => src.CardTypes.Select(ct => ct.Type.Name)))
                .ForMember(dto => dto.ImageUrl, opt => opt.MapFrom(src => src.OriginalImageUrl));
        }
    }
    }

