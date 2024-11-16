using CityInfo.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]

    public class CitiesController : ControllerBase
    // Let have the CitiesController class derives from the ControllerBase
    {
        private CitiesDataStore _citiesDataStore;

        //[HttpGet]

        //// json result class: this result a jsonifies version of whatever we passed into the cosntuctor of jsonresult
        //public JsonResult GetCities()
        //{
        //    // because we dont have the model class yet,so we gona create objects
        //    // id and a name

        //    //return new JsonResult(

        //    //    new List<object> 
        //    //    {
        //    //        new {id = 1, Name = "New York City" },
        //    //        new {id = 2, Name = "Antwerp" }
        //    //    }

        //    //    );

        //    // ** This action called all the Cities
        //    return new JsonResult(CitiesDataStore.Current.Cities);
        //}

        ////** Another action to call single city - this time we want to accept the parameter
        //[HttpGet("{id}")] // accept the id
        //public JsonResult GetCity(int id)
        //{
        //    return new JsonResult(
        //        CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id));
        //}

        //** we dont have to return the result in JSon format
        public CitiesController(CitiesDataStore citiesDataStore)
            {
                _citiesDataStore = citiesDataStore;
            }

        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(_citiesDataStore.Cities);
        }

        //** Return action result
        [HttpGet("{id}")] 
        public ActionResult<CityDto> GetCity(int id)
        {
            //Find City
            var cityToReturn = _citiesDataStore.Cities.
                FirstOrDefault(c => c.Id == id);
            

            if (cityToReturn == null)
            {
                return NotFound(); // This will return a 404 if the city isn't found
            }

            return Ok(cityToReturn); // This will return a 200 with the city data
        }

    }

}

