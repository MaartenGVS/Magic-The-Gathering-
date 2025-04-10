using AutoMapper;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO.CardDT0;
using Howest.MagicCards.Shared.DTO.SetDTO;
using Howest.MagicCards.Shared.DTO.TypeDTO;
using Howest.MagicCards.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Type = Howest.MagicCards.DAL.Models.Type;

namespace Howest.MagicCards.WebAPI.Controllers
{

    [ApiVersion("1.1")]
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {

        private readonly ITypeRepository _typeRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;


        public TypesController(ITypeRepository typeRepository, IMapper mapper, IDistributedCache memoryCache)
        {
            _typeRepository = typeRepository;
            _mapper = mapper;
            _cache = memoryCache;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TypeReadDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TypeReadDTO>>> GetTypes()
        {
            string cacheKey = "AllTypes";
            IEnumerable<TypeReadDTO> cachedTypes = await _cache.GetCachedDataAsync<IEnumerable<TypeReadDTO>>(cacheKey);

            if (cachedTypes != null)
            {
                return Ok(cachedTypes);
            }

            try
            {
                IEnumerable<Type> types = await _typeRepository.GetAllTypesAsync();
                IEnumerable<TypeReadDTO> typeReadDTOs = _mapper.Map<IEnumerable<TypeReadDTO>>(types);
                await _cache.SetCacheDataAsync(cacheKey, typeReadDTOs, TimeSpan.FromSeconds(60));
                return Ok(typeReadDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TypeReadDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TypeReadDTO>> GetTypeById(int id)
        {
            string cacheKey = $"Type{id}";
            TypeReadDTO cachedType = await _cache.GetCachedDataAsync<TypeReadDTO>(cacheKey);

            if (cachedType != null)
            {
                return Ok(cachedType);
            }

            try
            {
                Type type = await _typeRepository.GetTypeByIdAsync(id);
                if (type == null)
                {
                    return NotFound($"Type with id {id} not found");
                }
                TypeReadDTO typeReadDTO = _mapper.Map<TypeReadDTO>(type);
                await _cache.SetCacheDataAsync(cacheKey, typeReadDTO, TimeSpan.FromSeconds(60));
                return Ok(typeReadDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

