using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO.ArtistDTO;
using Howest.MagicCards.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [ApiVersion("1.1")]
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {

        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;


        public ArtistsController(IArtistRepository artistRepository, IMapper mapper, IDistributedCache memoryCache)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
            _cache = memoryCache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ArtistReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ArtistReadDTO>>> GetArtists()
        {
            string cacheKey = "AllArtists";
            IEnumerable<ArtistReadDTO> cachedArtists = await _cache.GetCachedDataAsync<IEnumerable<ArtistReadDTO>>(cacheKey);

            if (cachedArtists != null)
            {
                return Ok(cachedArtists);
            }

            try
            {
                IEnumerable<Artist> artists = await _artistRepository.GetAllArtistsAsync();
                IEnumerable<ArtistReadDTO> artistReadDTOs = _mapper.Map<IEnumerable<ArtistReadDTO>>(artists);
                await _cache.SetCacheDataAsync(cacheKey, artistReadDTOs, TimeSpan.FromSeconds(60));
                return Ok(artistReadDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ArtistReadDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ArtistReadDTO>> GetArtistById(int id)
        {
            string cacheKey = $"Artist_{id}";
            ArtistReadDTO cachedArtist = await _cache.GetCachedDataAsync<ArtistReadDTO>(cacheKey);

            if (cachedArtist != null)
            {
                return Ok(cachedArtist);
            }

            try
            {
                Artist artist = await _artistRepository.GetArtistByIdAsync(id);
                if (artist == null)
                {
                    return NotFound($"Artist with id {id} not found");
                }
                ArtistReadDTO artistReadDTO = _mapper.Map<ArtistReadDTO>(artist);
                await _cache.SetCacheDataAsync(cacheKey, artistReadDTO, TimeSpan.FromSeconds(60));
                return Ok(artistReadDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
