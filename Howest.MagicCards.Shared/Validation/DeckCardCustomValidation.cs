using FluentValidation;
using Howest.MagicCards.Shared.DTO.DeckDTO;

namespace Howest.MagicCards.Shared.Validation
{
    public class DeckCardCustomValidation : AbstractValidator<DeckEntryWriteDTO>
    {
        public DeckCardCustomValidation()
        {

            RuleFor(deckCard => deckCard.Card).NotNull().WithMessage("Card must be provided.");
            RuleFor(deckCard => deckCard.Card.Id).NotNull().GreaterThan(0);
            RuleFor(deckCard => deckCard.Card.Name).NotEmpty().WithMessage("Name must be provided.");
            RuleFor(deckCard => deckCard.Card.ManaCost).SetValidator(new ManaCostAttributeValidator());
        }
    }
}