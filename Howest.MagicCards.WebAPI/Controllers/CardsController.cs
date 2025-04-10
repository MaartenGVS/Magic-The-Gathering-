using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO.ArtistDTO;
using Howest.MagicCards.Shared.DTO.CardDT0;
using Howest.MagicCards.Shared.Extensions;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.WebAPI.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [ApiVersion("1.1")]
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;


        public CardsController(ICardRepository cardRepository, IMapper mapper, IDistributedCache memoryCache)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
            _cache = memoryCache;
        }

        [HttpGet, MapToApiVersion("1.1")]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<CardReadDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponse<IEnumerable<CardReadDTO>>>> GetCards(
                                                                            [FromQuery] CardFilter filter,
                                                                            [FromServices] IConfiguration _config
                                                                          )
        {
            filter.MaxPageSize = int.Parse(_config["maxPageSize"]);

            string cacheKey = GenerateCacheKey(filter);
            PagedResponse<IEnumerable<CardReadDTO>> cachedResponse = await _cache.GetCachedDataAsync<PagedResponse<IEnumerable<CardReadDTO>>>(cacheKey);

            if (cachedResponse != null)
            {
                return Ok(cachedResponse);
            }

            try
            {
                IEnumerable<Card> cards = await _cardRepository.GetAllCardsAsync();
                IQueryable<Card> filteredCardsQuery = cards
                                                         .AsQueryable()
                                                         .ToFilteredList(
                                                                           filter.SetName ?? string.Empty,
                                                                           filter.ArtistName ?? string.Empty,
                                                                           filter.RarityName ?? string.Empty,
                                                                           filter.CardTypeName ?? string.Empty,
                                                                           filter.CardName ?? string.Empty,
                                                                           filter.CardText ?? string.Empty
                                                                        );
                IQueryable<CardReadDTO> pagedCardsQuery = filteredCardsQuery
                                                                   .ProjectTo<CardReadDTO>(_mapper.ConfigurationProvider)
                                                                   .ToPagedList(filter.PageNumber, filter.PageSize);

                PagedResponse<IEnumerable<CardReadDTO>> pagedResponse = new(
                                                                              pagedCardsQuery,
                                                                              filter.PageNumber,
                                                                              filter.PageSize
                                                                           )
                {
                    TotalRecords = filteredCardsQuery.Count()
                };

                await CacheResponse(cacheKey, pagedResponse);

                return Ok(pagedResponse);
            }
            catch (Exception e)
            {
                return SendInternalServerError(e);
            }
        }


        [HttpGet, MapToApiVersion("1.5")]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<CardReadDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponse<IEnumerable<CardReadDTO>>>> GetCards(
                                                                            [FromQuery] CardFilter filter,
                                                                            [FromQuery] string orderBy,
                                                                            [FromQuery] string orderDirection,
                                                                            [FromServices] IConfiguration _config
                                                                          )
        {
            filter.MaxPageSize = int.Parse(_config["maxPageSize"]);

            string cacheKey = GenerateCacheKey(filter, orderBy, orderDirection);
            PagedResponse<IEnumerable<CardReadDTO>> cachedResponse = await _cache.GetCachedDataAsync<PagedResponse<IEnumerable<CardReadDTO>>>(cacheKey);

            if (cachedResponse != null)
            {
                return Ok(cachedResponse);
            }

            try
            {
                IEnumerable<Card> cards = await _cardRepository.GetAllCardsAsync();

                IQueryable<Card> filteredSortedCardsQuery = cards
                                                         .AsQueryable()
                                                            .Sort(orderBy, orderDirection)
                                                            .ToFilteredList(
                                                                              filter.SetName ?? string.Empty,
                                                                              filter.ArtistName ?? string.Empty,
                                                                              filter.RarityName ?? string.Empty,
                                                                              filter.CardTypeName ?? string.Empty,
                                                                              filter.CardName ?? string.Empty,
                                                                              filter.CardText ?? string.Empty
                                                                            );


                IQueryable<CardReadDTO> pagedCardsQuery = filteredSortedCardsQuery
                                                                    .ProjectTo<CardReadDTO>(_mapper.ConfigurationProvider)
                                                                    .ToPagedList(filter.PageNumber, filter.PageSize);

                PagedResponse<IEnumerable<CardReadDTO>> pagedResponse = new PagedResponse<IEnumerable<CardReadDTO>>(
                    pagedCardsQuery,
                    filter.PageNumber,
                    filter.PageSize
                )
                {
                    TotalRecords = filteredSortedCardsQuery.Count()
                };

                await CacheResponse(cacheKey, pagedResponse);

                return Ok(pagedResponse);
            }
            catch (Exception e)
            {
                return SendInternalServerError(e);
            }

        }

        [HttpGet("{id}"), MapToApiVersion("1.5")]
        [ProducesResponseType(typeof(CardReadDetailDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<CardReadDetailDTO>> GetCardById(long id)
        {
            string cacheKey = $"Card_{id}";
            CardReadDetailDTO cachedCard = await _cache.GetCachedDataAsync<CardReadDetailDTO>(cacheKey);

            if (cachedCard != null)
            {
                return Ok(cachedCard);
            }

            try
            {
                Card card = await _cardRepository.GetCardByIdAsync(id);
                if (card == null)
                {
                    return NotFound();
                }

                CardReadDetailDTO cardReadDetailDTO = _mapper.Map<CardReadDetailDTO>(card);
                await _cache.SetCacheDataAsync(cacheKey, cardReadDetailDTO, TimeSpan.FromSeconds(60));
                return Ok(cardReadDetailDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        private async Task CacheResponse(string cacheKey, object response)
        {
            var serializedResponse = JsonConvert.SerializeObject(response);
            await _cache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
            });
        }

        private ActionResult SendInternalServerError(Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {e}");
        }

        private string GenerateCacheKey(CardFilter filter, string orderBy = null, string orderDirection = null)
        {
            return $"AllCards_{filter.PageNumber}_{filter.PageSize}_{filter.SetName}_{filter.ArtistName}_{filter.RarityName}_{filter.CardTypeName}_{filter.CardName}_{filter.CardText}_{orderBy}-{orderDirection}";
        }
    }
}
