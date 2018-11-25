using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Entities;
using CityInfo.Models;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        #region Fields

        private CityInfoContext mDbContext;

        #endregion


        #region Init and clean-up

        public CityInfoRepository(CityInfoContext dbContext)
        {
            mDbContext = dbContext;
        }

        #endregion


        #region ICityInfoRepository implementation

        IEnumerable<City> ICityInfoRepository.GetCities()
        {
            return mDbContext.Cities.OrderBy(c => c.Name).ToList();
        }


        bool ICityInfoRepository.DoesCityExist(int cityId)
        {
            return mDbContext.Cities.Any(c => c.Id == cityId);
        }


        City ICityInfoRepository.GetCity(int cityId, bool includePois)
        {
            if (includePois)
            {
                return mDbContext.Cities.Include(c => c.Poi).Where(c => c.Id == cityId).FirstOrDefault();
            }

            return mDbContext.Cities.Where(c => c.Id == cityId).FirstOrDefault();
        }


        IEnumerable<Poi> ICityInfoRepository.GetPois(int cityId)
        {
            return mDbContext.Pois.Where(p => p.CityId == cityId).ToList();
        }


        Poi ICityInfoRepository.GetPoi(int cityId, int poiId)
        {
            return mDbContext.Pois.Where(p => p.CityId == cityId && p.Id == poiId).FirstOrDefault();
        }


        void ICityInfoRepository.AddPoi(int cityId, Poi poi)
        {
            var city = (this as ICityInfoRepository).GetCity(cityId, false);
            city.Poi.Add(poi);
        }


        void ICityInfoRepository.DeletePoi(Poi poi)
        {
            mDbContext.Pois.Remove(poi);
        }


        bool ICityInfoRepository.Save()
        {
            return mDbContext.SaveChanges() >= 0;
        }

        #endregion
    }
}
