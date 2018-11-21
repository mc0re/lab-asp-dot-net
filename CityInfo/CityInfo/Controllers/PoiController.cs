using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CityInfo.Controllers
{
    [Route("api/cities")]
    public class PoiController : Controller
    {
        [HttpGet("{cityId}/poi")]
        public IActionResult GetPois(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) return NotFound();

            return Ok(city.Poi);
        }


        [HttpGet("{cityId}/poi/{id}")]
        public IActionResult GetPoi(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) return NotFound();

            var poi = city.Poi.FirstOrDefault(p => p.Id == id);
            if (poi == null) return NotFound();

            return Ok(poi);
        }
    }
}
