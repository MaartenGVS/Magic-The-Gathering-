using Howest.MagicCards.DAL.Models;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class CardExtensions
    {

        public static IQueryable<T> WhereIf<T>(
                                                 this IQueryable<T> source,
                                                 bool condition,
                                                 Expression<Func<T, bool>> predicate
                                              )
        {
            if (condition)
            {
                return source.Where(predicate);
            }

            return source;
        }


        public static string ConvertManaCost(string manaCost)
        {
            if (string.IsNullOrWhiteSpace(manaCost))
            {
                return string.Empty;
            }
            Regex regex = new(@"\{(.*?)\}");
            return regex.Replace(manaCost, m => m.Groups[1].Value.ToLower());
        }


        public static string GetManaColors(string manaCost)
        {
            if (string.IsNullOrWhiteSpace(manaCost))
            {
                return string.Empty;
            }

            Regex regex = new Regex(@"\{(.*?)\}");
            HashSet<string> manaColors = new();

            foreach (Match match in regex.Matches(manaCost))
            {
                string value = match.Groups[1].Value.ToLower();
                if (!value.Any(char.IsDigit))
                {
                    manaColors.Add(value);
                }
            }

            return string.Join("", manaColors.OrderBy(color => color));
        }


        public static IQueryable<Card> ToFilteredList(
                                                        this IQueryable<Card> entities,
                                                        string setName,
                                                        string artistName,
                                                        string rarityName,
                                                        string cardTypeName,
                                                        string cardName,
                                                        string cardText
                                                    )
        {
            return entities
                      .WhereIf(setName != string.Empty, c => c.Set.Name == setName)
                      .WhereIf(artistName != string.Empty, c => c.Artist.FullName == artistName)
                      .WhereIf(rarityName != string.Empty, c => c.Rarity.Name == rarityName)
                      .WhereIf(cardTypeName != string.Empty, c => c.Type.Contains(cardTypeName))
                      .WhereIf(cardName != string.Empty, c => c.Name == cardName)
                      .WhereIf(cardText != string.Empty, c => c.Text.Contains(cardText));
        }
    }
}