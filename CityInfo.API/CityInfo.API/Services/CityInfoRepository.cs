﻿using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    // this class to implement our contract
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        // inject a city in full context using construct
        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includedPointsOfInterest)
        {
            if (includedPointsOfInterest)
            {
                return await _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }
            return await _context.Cities
                .Where(c => c.Id == cityId).FirstOrDefaultAsync();

        }

        public async Task<bool>  CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id ==cityId);
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(
            int cityId,
            int pointofInterestId)
        {
            return await _context.PointsOfInterest
                    .Where(p => p.CityId == cityId && p.Id == pointofInterestId)
                    .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(
            int cityId)
        {
            return await _context.PointsOfInterest
                .Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task AddPointOfInterestForCityAsync(int cityId,
            PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            if (city != null)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}

