using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using CityInfo.API.Models;



namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        // return a list of points of interest
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(
            int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(c => c.Id == cityId);
            if (city == null)

            {
                return NotFound();
            }


            //Find point of Interest
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }
    }
}
