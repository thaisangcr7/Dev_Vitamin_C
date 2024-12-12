using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using CityInfo.API.Models;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.JsonPatch;
using CityInfo.API.Services;
using AutoMapper;



namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        // Controller Injection _ Logger
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;



        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? 
                throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? 
                throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        // return a list of points of interest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(
            int cityId)
        {

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasnt found when accessing points of interest.");
                return NotFound();
            }

            var pointsOfInterestForCity = await _cityInfoRepository
                .GetPointsOfInterestForCityAsync(cityId);
            
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));

        }
         


        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
            int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
        
            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));

        }

        // Create a new resource
        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterestAsync(
            int cityId,
            PointOfInterestForCreationDto pointOfInterest)
        {

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            // Mapping 
            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(
                cityId, finalPointOfInterest );

            await _cityInfoRepository.SaveChangesAsync();

            var createPointOfInterestToReturn =
                _mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = createPointOfInterestToReturn.Id
                },
                createPointOfInterestToReturn);
        }

        //// Full update a resource
        //[HttpPut("{pointofinterestid}")]
        //public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId,
        //    PointOfInterestForUpdateDto pointOfInterest)
        //{
        //    var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        //    if(city == null)
        //    {
        //        return NotFound();
        //    }
        //    //Find point of Interest
        //    var pointOfInterestFromStore = city.PointsOfInterest
        //        .FirstOrDefault(c => c.Id == pointOfInterestId);

        //    if (pointOfInterestFromStore == null)
        //    {
        //        return NotFound();
        //    }

        //    pointOfInterestFromStore.Name = pointOfInterest.Name;
        //    pointOfInterestFromStore.Description = pointOfInterest.Description;

        //    return NoContent();

        //    }

        //[HttpPatch("{pointofinterestid}")]

        //public ActionResult PartiallyUpdatePointOfInterest(
        //    int cityId, int pointOfInterestId,
        //    JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        //{
        //    var city = _citiesDataStore.Cities.
        //    FirstOrDefault(c => c.Id == cityId);
        //    if (city == null)
        //    {
        //         return NotFound(); 
        //    }

        //    var pointOfInterestFromStore = city.PointsOfInterest.
        //    FirstOrDefault(c => c.Id == pointOfInterestId); 
        //    if (pointOfInterestFromStore == null)
        //    {
        //        return NotFound();
        //    }

        //    var pointOfInterestToPatch = 
        //        new PointOfInterestForUpdateDto()
        //        {
        //            Name = pointOfInterestFromStore.Name,
        //            Description = pointOfInterestFromStore.Description,

        //        };

        //    patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (!TryValidateModel(pointOfInterestToPatch))
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
        //    pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

        //    return NoContent();
        //}

        //// Delete a reource using the point of interestid 
        //// check if the city and point of interest for that city exist
        //// and if they don't, we return NOt Found
        //// if they do, we delete the point of interest from the city

        //[HttpDelete("{pointOfInterestId}")]

        //public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        //{
        //    var city = _citiesDataStore.Cities.
        //    FirstOrDefault(c => c.Id == cityId);

        //    if (city == null)
        //    {
        //        return NotFound();
        //    }

        //    var pointOfInterestFromStore = city.PointsOfInterest.
        //    FirstOrDefault(c => c.Id == pointOfInterestId);
        //    if (pointOfInterestFromStore == null)
        //    {
        //        return NotFound();
        //    }

        //    city.PointsOfInterest.Remove(pointOfInterestFromStore);

        //    _mailService.Send("Point of interest deleted.",
        //        $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted.");

        //    return NoContent();
        //}




    }
}
