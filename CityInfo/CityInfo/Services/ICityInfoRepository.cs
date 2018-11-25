using CityInfo.Entities;
using CityInfo.Models;
using System.Collections.Generic;

namespace CityInfo.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();

        bool DoesCityExist(int cityId);

        City GetCity(int cityId, bool includePois);

        IEnumerable<Poi> GetPois(int cityId);

        Poi GetPoi(int cityId, int poiId);

        void AddPoi(int cityId, Poi poi);

        void DeletePoi(Poi poi);

        bool Save();
    }
}
