using CityInfo.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CityInfo.Controllers
{
    [Route("api/cities")]
    public class PoiController : Controller
    {
        private static CityDto GetCityOrNothing(int cityId)
        {
            return CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        }


        [HttpGet("{cityId}/poi")]
        public IActionResult GetPois(int cityId)
        {
            var city = GetCityOrNothing(cityId);
            if (city == null) return NotFound();

            return Ok(city.Poi);
        }


        [HttpGet("{cityId}/poi/{id}", Name = "GetPoi")]
        public IActionResult GetPoi(int cityId, int id)
        {
            var city = GetCityOrNothing(cityId);
            if (city == null) return NotFound();

            var poi = city.Poi.FirstOrDefault(p => p.Id == id);
            if (poi == null) return NotFound();

            return Ok(poi);
        }


        [HttpPost("{cityId}/poi")]
        public IActionResult CreatePoi(int cityId, [FromBody] PoiNewDto poi)
        {
            if (poi == null)
            {
                ModelState.AddModelError("POI", "The input object cannot be deserialized");
            }
            else if (poi.Name == poi.Description)
            {
                ModelState.AddModelError("Description", "Description shall differ from the name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = GetCityOrNothing(cityId);
            if (city == null) return NotFound();

            var nextId = CitiesDataStore.Current.Cities.SelectMany(c => c.Poi).Max(p => p.Id) + 1;

            var res = new PoiDto
            {
                Id = nextId,
                Name = poi.Name,
                Description = poi.Description
            };

            city.Poi.Add(res);

            return CreatedAtRoute("GetPoi", new { cityId = city.Id, id = nextId }, res);
        }


        [HttpPut("{cityId}/poi/{id}")]
        public IActionResult UpdatePoi(int cityId, int id, [FromBody] PoiUpdateDto poi)
        {
            var validator = new PoiValidator();
            var validation = validator.Validate(poi);
            validation.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = GetCityOrNothing(cityId);
            if (city == null) return NotFound(new { CityId = cityId });

            var existingPoi = city.Poi.FirstOrDefault(p => p.Id == id);
            if (existingPoi == null) return NotFound(new { PoiId = id });

            existingPoi.Name = poi.Name;
            existingPoi.Description = poi.Description;

            return NoContent();
        }
    }
}
