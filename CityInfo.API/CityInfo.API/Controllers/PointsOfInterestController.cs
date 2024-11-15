using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using CityInfo.API.Models;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.JsonPatch;



namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        // Controller Injection
        private readonly ILogger<PointsOfInterestController> _logger;
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
        }

        // return a list of points of interest
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                _logger.LogInformation($"City with Id {cityId} wasnt found whrn accessing point of interest.");
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }


        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)


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
        
        // Create a new resource
        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(
            int cityId,
            PointOfInterestForCreationDto pointOfInterest)
            {

                var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    return NotFound();
                }

                // Demo purposes - to be improved
                var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
                    c=> c.PointsOfInterest).Max(p => p.Id);

                var finalPointOfInterest = new PointOfInterestDto()
                {
                    Id = ++maxPointOfInterestId,
                    Name = pointOfInterest.Name,
                    Description = pointOfInterest.Description,
                };

            city.PointsOfInterest.Add(finalPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = finalPointOfInterest.Id
                },
                finalPointOfInterest);   
            }

        // Full update a resource
        [HttpPut("{pointofinterestid}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterest)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if(city == null)
            {
                return NotFound();
            }
            //Find point of Interest
            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(c => c.Id == pointOfInterestId);

            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();

            }

        [HttpPatch("{pointofinterestid}")]

        public ActionResult PartiallyUpdatePointOfInterest(
            int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = CitiesDataStore.Current.Cities.
            FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                 return NotFound(); 
            }

            var pointOfInterestFromStore = city.PointsOfInterest.
            FirstOrDefault(c => c.Id == pointOfInterestId); 
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = 
                new PointOfInterestForUpdateDto()
                {
                    Name = pointOfInterestFromStore.Name,
                    Description = pointOfInterestFromStore.Description,

                };
            
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        // Delete a reource using the point of interestid 
        // check if the city and point of interest for that city exist
        // and if they don't, we return NOt Found
        // if they do, we delete the point of interest from the city

        [HttpDelete("{pointOfInterestId}")]

        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.
            FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.
            FirstOrDefault(c => c.Id == pointOfInterestId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);
            return NoContent();
        }

        
        

    }
}
