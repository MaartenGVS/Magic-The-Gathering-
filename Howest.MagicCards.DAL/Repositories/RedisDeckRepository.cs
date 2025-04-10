using Howest.MagicCards.DAL.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Howest.MagicCards.DAL.Repositories
{
    public class RedisDeckRepository : IDeckRepository
    {

        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisDeckRepository(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }


        public async Task<IEnumerable<DeckEntry>> GetDeckEntriesAsync()
        {
            IEnumerable<RedisKey> deckCardKeys = GetDeckCardKeys();
            IEnumerable<DeckEntry> deckCards = await GetDeckCardsFromKeysAsync(deckCardKeys);
            return deckCards.Where(deckCard => deckCard != null);
        }

        public async void AddDeckEntryAsync(DeckEntry deckCard)
        {
            await _db.StringSetAsync(deckCard.EntryId, JsonSerializer.Serialize(deckCard));
        }

        public async Task<DeckEntry> GetDeckCardByCardIdAsync(long cardId)
        {
            IEnumerable<DeckEntry> deckCards = await GetDeckEntriesAsync();
            return deckCards.FirstOrDefault(deckCard => deckCard.Card.Id == cardId);
        }

        public async Task<DeckEntry> GetDeckEntryByIdAsync(string entryId)
        {
            RedisValue deckCardJson = await _db.StringGetAsync(entryId);
            return deckCardJson.IsNullOrEmpty ? null : JsonSerializer.Deserialize<DeckEntry>(deckCardJson);
        }

        public async void UpdateDeckEntryAsync(DeckEntry deckCard)
        {
            await _db.StringSetAsync(deckCard.EntryId, JsonSerializer.Serialize(deckCard));
        }

        public async void DeleteDeckEntryAsync(DeckEntry deckCard)
        {
            await _db.KeyDeleteAsync(deckCard.EntryId);
        }

        public async void DeleteAllEntriesAsync()
        {   
            await _db.KeyDeleteAsync(GetDeckCardKeys().ToArray());
        }

        private IEnumerable<RedisKey> GetDeckCardKeys()
        {
            return _db.Multiplexer.GetServer(_redis.GetEndPoints().First())
                                  .Keys(pattern: "deckEntry:*");
        }

        private async Task<IEnumerable<DeckEntry>> GetDeckCardsFromKeysAsync(IEnumerable<RedisKey> keys)
        {
            IEnumerable<Task<DeckEntry>> tasks = keys.Select(async key =>
                                       {
                                            var deckCardJson = await _db.StringGetAsync(key);
                                            return deckCardJson.IsNullOrEmpty ? null : JsonSerializer.Deserialize<DeckEntry>(deckCardJson);
                                       }
                                   );
            DeckEntry[] deckCards = await Task.WhenAll(tasks);
            return deckCards.Where(deckCard => deckCard != null);
        }
    }
}
