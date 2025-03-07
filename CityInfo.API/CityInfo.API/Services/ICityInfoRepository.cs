﻿using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        // Need a method to get to the cities 
        //IEnumerable<City> GetCities(); //- this is a synchronous method
        Task<IEnumerable<City>> GetCitiesAsync(); // this is a Async method

        Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int PageNumber, int pageSize);
        
        //a method to get 1 city
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest); // can return null if the city isnt found

        Task<bool> CityExistsAsync(int cityId);
        // get a point of interest
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);

        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId,
            int pointofInterestId);

        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        Task<bool> CityNameMatchesCityId(string? cityName, int cityId);
        Task<bool> SaveChangesAsync();
    }
}
