using AutoMapper;
using CityInfo.Models;
using CityInfo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        #region Fields

        private ICityInfoRepository mCityRepo;

        #endregion


        #region Init and clean-up

        public CitiesController(ICityInfoRepository cityRepo)
        {
            mCityRepo = cityRepo;
        }

        #endregion


        #region HTTP requests

        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok(Mapper.Map<IEnumerable<CityNoPoiDto>>(mCityRepo.GetCities()));
        }


        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePois = false)
        {
            var city = mCityRepo.GetCity(id, includePois);
            if (city == null) return NotFound();

            if (includePois)
            {
                return Ok(Mapper.Map<CityDto>(city));
            }
            else
            {
                return Ok(Mapper.Map<CityNoPoiDto>(city));
            }
        }

        #endregion
    }
}
