using FluentValidation;
using FluentValidation.Validators;
using Howest.MagicCards.Shared.DTO.DeckDTO;
using System.Text.RegularExpressions;

namespace Howest.MagicCards.Shared.Validation
{
    public class ManaCostAttributeValidator : PropertyValidator<DeckEntryWriteDTO, string>
    {
        private const string _manaCostPattern = @"^(\{[0-9]+\}|\{[WUBRG]\})*$";

        public override string Name => "ManaCostValidator";

        public override bool IsValid(ValidationContext<DeckEntryWriteDTO> context, string value)
        {
            if (Regex.IsMatch(value, _manaCostPattern))
            {
                return true;
            }

            return false;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return "Invalid mana cost format. Valid formats include numeric values and symbols like {W}, {U}, {B}, {R}, {G}.";
        }
    }
}