using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO.ColorDTO;
using Howest.MagicCards.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Howest.MagicCards.WebAPI.Controllers
{

    [ApiVersion("1.1")]
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;


        public ColorsController(IColorRepository colorRepository, IMapper mapper, IDistributedCache memoryCache)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
            _cache = memoryCache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ColorReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ColorReadDTO>>> GetColors()
        {
            string cacheKey = "AllColors";
            IEnumerable<ColorReadDTO> cachedColors = await _cache.GetCachedDataAsync<IEnumerable<ColorReadDTO>>(cacheKey);

            if (cachedColors != null)
            {
                return Ok(cachedColors);
            }

            try
            {
                IEnumerable<Color> colors = await _colorRepository.GetAllColorsAsync();
                IEnumerable<ColorReadDTO> colorsReadDTOs = _mapper.Map<IEnumerable<ColorReadDTO>>(colors);
                await _cache.SetCacheDataAsync(cacheKey, colorsReadDTOs, TimeSpan.FromSeconds(60));
                return Ok(colorsReadDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ColorReadDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ColorReadDTO>> GetColorById(int id)
        {
            string cacheKey = $"Color{id}";
            ColorReadDTO cachedColor = await _cache.GetCachedDataAsync<ColorReadDTO>(cacheKey);

            if (cachedColor != null)
            {
                return Ok(cachedColor);
            }

            try
            {
                Color color = await _colorRepository.GetColorByIdAsync(id);
                if (color == null)
                {
                    return NotFound($"Color with id {id} not found");
                }
                ColorReadDTO colorReadDTO = _mapper.Map<ColorReadDTO>(color);
                await _cache.SetCacheDataAsync(cacheKey, colorReadDTO, TimeSpan.FromSeconds(60));
                return Ok(colorReadDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
