using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO.DeckDTO;

namespace Howest.MagicCards.Shared.Mappings
{
    public class DeckCardsProfile : Profile
    {
        public DeckCardsProfile()
        {
            CreateMap<DeckEntry, DeckEntryReadDTO>()
                .ForMember(dest => dest.EntryId, opt => opt.MapFrom(src => src.EntryId))
                .ForMember(dest => dest.Card, opt => opt.MapFrom(src => src.Card))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));


            CreateMap<DeckCard, DeckCardReadDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ManaCost, opt => opt.MapFrom(src => src.ManaCost));


            CreateMap<DeckCard, DeckEntryReadDTO>()
                .ForMember(dest => dest.EntryId, opt => opt.Ignore())
                .ForMember(dest => dest.Card, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Quantity, opt => opt.Ignore());


            CreateMap<DeckEntryWriteDTO, DeckCard>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Card.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Card.Name))
                .ForMember(dest => dest.ManaCost, opt => opt.MapFrom(src => src.Card.ManaCost));


            CreateMap<DeckEntryWriteDTO, DeckEntry>()
                .ForMember(dest => dest.EntryId, opt => opt.AllowNull())
                .ForMember(dest => dest.Card, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Quantity, opt => opt.AllowNull());
        }
    }
}

