using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO.ArtistDTO;
using Howest.MagicCards.Shared.DTO.CardDT0;
using Howest.MagicCards.Shared.DTO.DeckDTO;
using Howest.MagicCards.Shared.DTO.SetDTO;
using Howest.MagicCards.Shared.DTO.TypeDTO;
using Howest.MagicCards.Web.Services;
using Howest.MagicCards.WebAPI.Wrappers;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Howest.MagicCards.Web.Components.Pages
{
    public partial class DeckBuilder
    {
        [Inject]
        public ICardService CardService { get; set; }

        [Inject]
        public IDeckService DeckService { get; set; }

        [Inject]
        protected IMatToaster Toaster { get; set; }

        [Inject]
        public ProtectedLocalStorage Storage { get; set; }


        private IEnumerable<ArtistReadDTO> _artists = Enumerable.Empty<ArtistReadDTO>();
        private IEnumerable<TypeReadDTO> _types = Enumerable.Empty<TypeReadDTO>();
        private IEnumerable<SetReadDTO> _sets = Enumerable.Empty<SetReadDTO>();
        private IEnumerable<CardReadDetailDTO> _cards = Enumerable.Empty<CardReadDetailDTO>();
        private IEnumerable<DeckEntryReadDTO> _deckEntries = Enumerable.Empty<DeckEntryReadDTO>();

        private int _currentPage = 1;
        private int _pageSize = 150;
        private int _totalPages;

        private bool _hasPrevDeck = false;

        private CardFilterParams _filterParams;
        private CardSortParams _sortParams = new CardSortParams("CardName", null);


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _artists = await CardService.GetAllArtistsAsync();
                _types = await CardService.GetAllTypesAsync();
                _sets = await CardService.GetAllSetsAsync();
                await FetchAndSetCardsAsync();

                _deckEntries = await DeckService.GetDeckEntriesAsync();

                ProtectedBrowserStorageResult<IEnumerable<DeckEntryReadDTO>> deck = await Storage.GetAsync<IEnumerable<DeckEntryReadDTO>>("deck");
                if (deck.Success)
                {
                    _hasPrevDeck = true;
                }

                StateHasChanged();
            }
        }


        private async Task FetchAndSetCardsAsync()
        {
            PagedResponse<IEnumerable<CardReadDetailDTO>> cards = await CardService.GetAllCardsAsync(_filterParams, _sortParams, _currentPage, _pageSize);
            _totalPages = cards.TotalPages;
            _cards = cards.Data;
        }


        #region Filter and Sort
        private async Task HandleApplyFilters(CardFilterParams filterParams)
        {
            _filterParams = filterParams;
            _currentPage = 1;
            await FetchAndSetCardsAsync();
        }

        private async Task HandleOrderByChange(string orderDirection)
        {
            _sortParams.OrderDirection = orderDirection;
            await FetchAndSetCardsAsync();
        }
        #endregion


        #region Pagination
        private async Task HandlePageChange(int page)
        {
            _currentPage = page;
            await FetchAndSetCardsAsync();
        }

        private async Task HandlePageSizeChange(int pageSize)
        {
            _pageSize = pageSize;
            _currentPage = 1;
            await FetchAndSetCardsAsync();
        }
        #endregion


        #region Deck Management
        private async Task HandleAddCardToDeck((long cardId, string cardName, string manaCost) card)
        {
            if (IsDeckFull())
            {
                Toaster.Add("You can't have more than 60 cards in your deck", MatToastType.Danger);
                return;
            }

            DeckEntryWriteDTO deckCardRequest = new()
            {
                EntryId = null,
                Card = new DeckCardReadDTO
                {
                    Id = card.cardId,
                    Name = card.cardName,
                    ManaCost = card.manaCost
                },
                Quantity = 1
            };

            await ModifyDeckAsync(async () => await DeckService.AddCardToDeckAsync(deckCardRequest));
        }

        private async Task HandleIncrementDeckEntry(long cardId)
        {
            if (IsDeckFull())
            {
                Toaster.Add("You can't have more than 60 cards in your deck", MatToastType.Danger);
                return;
            }

            await ModifyDeckAsync(async () => await DeckService.IncrementDeckEntryAsync(cardId));
        }

        private async Task HandleDecrementDeckEntry(long cardId)
        {
            await ModifyDeckAsync(async () => await DeckService.DecrementDeckEntryAsync(cardId));
        }

        private async Task HandleClearDeck()
        {
            await ModifyDeckAsync(async () =>
            {
                await Storage.SetAsync("deck", _deckEntries);
                _hasPrevDeck = true;
                await DeckService.ClearDeckAsync();
            }, "Deck cleared", "Failed to clear deck");
        }

        private async Task HandleResetPrevDeck()
        {
            ProtectedBrowserStorageResult<IEnumerable<DeckEntryReadDTO>> deck = await Storage.GetAsync<IEnumerable<DeckEntryReadDTO>>("deck");
            if (!deck.Success)
            {
                ShowToast("Failed to reset deck", MatToastType.Warning);
                return;
            }

            IEnumerable<Task> deckEntries = deck.Value.Select(async deckEntry =>
            {
                DeckEntryWriteDTO deckCardRequest = new DeckEntryWriteDTO
                {
                    EntryId = deckEntry.EntryId,
                    Card = new DeckCardReadDTO
                    {
                        Id = deckEntry.Card.Id,
                        Name = deckEntry.Card.Name,
                        ManaCost = deckEntry.Card.ManaCost
                    },
                    Quantity = deckEntry.Quantity
                };

                try
                {
                    await DeckService.AddCardToDeckAsync(deckCardRequest);
                }
                catch (HttpRequestException)
                {
                    ShowToast("Failed to reset deck", MatToastType.Warning);
                }
            });

            await Task.WhenAll(deckEntries);

            _deckEntries = deck.Value;
            await Storage.DeleteAsync("deck");
            _hasPrevDeck = false;
            ShowToast("Deck reset", MatToastType.Success);
        }

        #endregion


        #region Helpers
        private bool IsDeckFull() => _deckEntries.Sum(entry => entry.Quantity) >= 60;

        private void ShowToast(string message, MatToastType type)
        {
            Toaster.Add(message, type);
        }

        private async Task ModifyDeckAsync(Func<Task> modifyAction, string successMessage = null, string errorMessage = "Failed to modify deck")
        {
            try
            {
                await modifyAction();
                _deckEntries = await DeckService.GetDeckEntriesAsync();
                if (successMessage != null)
                {
                    ShowToast(successMessage, MatToastType.Success);
                }
            }
            catch (HttpRequestException)
            {
                ShowToast(errorMessage, MatToastType.Warning);
            }
        }
        #endregion
    }
}
