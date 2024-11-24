using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        // Need a method to get to the cities 
        //IEnumerable<City> GetCities(); //- this is a synchronous method
        Task<IEnumerable<City>> GetCitiesAsync(); // this is a Async method
        
        //a method to get 1 city
        Task<City?> GetCityAsync(int cityId); // can return null if the city isnt found

        // get a point of interest
        Task<IEnumerable<PointOfInterest?>> GetPointsOfInterestForCityAsync(int cityId, 
            int pointOfInterestId);


    }
}
