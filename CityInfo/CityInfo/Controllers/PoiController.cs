using CityInfo.Models;
using CityInfo.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CityInfo.Controllers
{
    [Route("api/cities")]
    public class PoiController : Controller
    {
        #region Fields

        private ILogger<PoiController> mLogger;

        private ISenderService mSender;

        private static CityDto GetCityOrNothing(int cityId)
        {
            return CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        }

        #endregion


        #region Init and clean-up

        public PoiController(ILogger<PoiController> logger, ISenderService sender)
        {
            mLogger = logger;
            mSender = sender;
        }

        #endregion


        #region HTTP requests

        [HttpGet("{cityId}/poi")]
        public IActionResult GetPois(int cityId)
        {
            var city = GetCityOrNothing(cityId);
            if (city == null)
            {
                mLogger.LogInformation($"City {cityId} not found.");
                return NotFound();
            }

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
            //var validator = new PoiValidator();
            //var validation = validator.Validate(poi);
            //validation.AddToModelState(ModelState, null);

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


        [HttpPatch("{cityId}/poi/{id}")]
        public IActionResult PatchPoi(int cityId, int id, [FromBody] JsonPatchDocument<PoiUpdateDto> patch)
        {
            if (patch == null)
            {
                return BadRequest();
            }

            var city = GetCityOrNothing(cityId);
            if (city == null) return NotFound(new { CityId = cityId });

            var existingPoi = city.Poi.FirstOrDefault(p => p.Id == id);
            if (existingPoi == null) return NotFound(new { PoiId = id });

            var patched = new PoiUpdateDto { Name = existingPoi.Name, Description = existingPoi.Description };
            patch.ApplyTo(patched, ModelState);
            TryValidateModel(patched);

            if (! ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            existingPoi.Name = patched.Name;
            existingPoi.Description = patched.Description;

            return NoContent();
        }


        [HttpDelete("{cityId}/poi/{id}")]
        public IActionResult DeletePoi(int cityId, int id)
        {
            var city = GetCityOrNothing(cityId);
            if (city == null) return NotFound(new { CityId = cityId });

            var existingPoi = city.Poi.FirstOrDefault(p => p.Id == id);
            if (existingPoi == null) return NotFound(new { PoiId = id });

            city.Poi.Remove(existingPoi);
            mSender.Send("POI deleted", $"POI {existingPoi.Id} '{existingPoi.Name}' was deleted.");
            return NoContent();
        }

        #endregion
    }
}
