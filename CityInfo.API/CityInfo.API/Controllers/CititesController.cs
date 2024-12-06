using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]

    public class CitiesController : ControllerBase
    // Let have the CitiesController class derives from the ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;

        //** we dont have to return the result in JSon format
        public CitiesController(ICityInfoRepository cityInfoRepository)
            {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();
            var results = new List<CityWithoutPointsOfInterestDto>();
            foreach (var cityEntity in cityEntities)
            {
                results.Add(new CityWithoutPointsOfInterestDto
                {
                    Id = cityEntity.Id,
                    Description = cityEntity.Description,
                    Name = cityEntity.Name
                });
            }
            return Ok(results);
            //return Ok(_citiesDataStore.Cities);
        }

        //** Return action result
        [HttpGet("{id}")] 
        public ActionResult<CityDto> GetCity(int id)
        {
            ////Find City
            //var cityToReturn = _citiesDataStore.Cities.
            //    FirstOrDefault(c => c.Id == id);


            //if (cityToReturn == null)
            //{
            //    return NotFound(); // This will return a 404 if the city isn't found
            //}

            //return Ok(cityToReturn); // This will return a 200 with the city data
            return Ok();
        }

    }

}

