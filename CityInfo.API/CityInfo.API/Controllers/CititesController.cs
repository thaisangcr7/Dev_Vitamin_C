using Asp.Versioning;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.API.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/v{version:apiVersion}/cities")]
    [ApiVersion(1)]
    [ApiVersion(2)]
    public class CitiesController : ControllerBase
    // Let have the CitiesController class derives from the ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        //** we dont have to return the result in JSon format
        public CitiesController(ICityInfoRepository cityInfoRepository,
            IMapper mapper)
            {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
            string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10 )
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }

            var (cityEntities, paginationMetadata) = await _cityInfoRepository
                .GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(paginationMetadata));
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
            
        }

        /// <summary>
        /// Get a City by id
        /// </summary>
        /// <param name="id"> The Id of the city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include the points of interest</param>
        /// <returns>A City with or without points of interest</returns>
        /// <response code="200">Returns the requested city</response>

        //** Return action result
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCity(
            int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }

    }

}

