using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO.SetDTO;
using Howest.MagicCards.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [ApiVersion("1.1")]
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SetsController : ControllerBase
    {

        private readonly ISetRepository _setRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;


        public SetsController(ISetRepository setRepository, IMapper mapper, IDistributedCache memoryCache)
        {
            _setRepository = setRepository;
            _mapper = mapper;
            _cache = memoryCache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SetReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SetReadDTO>>> GetSets()
        {
            string cacheKey = "AllSets";
            IEnumerable<SetReadDTO> cachedSets = await _cache.GetCachedDataAsync<IEnumerable<SetReadDTO>>(cacheKey);

            if (cachedSets != null)
            {
                return Ok(cachedSets);
            }

            try
            {
                IEnumerable<Set> sets = await _setRepository.GetAllSetsAsync();
                IEnumerable<SetReadDTO> setReadDTOs = _mapper.Map<IEnumerable<SetReadDTO>>(sets);
                await _cache.SetCacheDataAsync(cacheKey, setReadDTOs, TimeSpan.FromSeconds(60));
                return Ok(setReadDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SetReadDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SetReadDTO>> GetSetById(int id)
        {
            string cacheKey = $"Set{id}";
            SetReadDTO cachedSet = await _cache.GetCachedDataAsync<SetReadDTO>(cacheKey);

            if (cachedSet != null)
            {
                return Ok(cachedSet);
            }

            try
            {
                Set set = await _setRepository.GetSetByIdAsync(id);
                if (set == null)
                {
                    return NotFound($"Set with id {id} not found");
                }
                SetReadDTO setReadDTO = _mapper.Map<SetReadDTO>(set);
                await _cache.SetCacheDataAsync(cacheKey, setReadDTO, TimeSpan.FromSeconds(60));
                return Ok(setReadDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}