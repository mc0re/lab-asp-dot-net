using AutoMapper;
using CityInfo.Entities;
using CityInfo.Models;
using CityInfo.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class PoiController : ControllerBase
    {
        #region Fields

        private ILogger<PoiController> mLogger;

        private ISenderService mSender;

        private ICityInfoRepository mCityRepo;

        private static CityDto GetCityOrNothing(int cityId)
        {
            return CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
        }

        #endregion


        #region Init and clean-up

        public PoiController(ILogger<PoiController> logger, ISenderService sender, ICityInfoRepository cityRepo)
        {
            mLogger = logger;
            mSender = sender;
            mCityRepo = cityRepo;
        }

        #endregion


        #region HTTP requests

        [HttpGet("{cityId}/poi")]
        public IActionResult GetPois(int cityId)
        {
            if (! mCityRepo.DoesCityExist(cityId))
            {
                mLogger.LogInformation($"City {cityId} not found.");
                return NotFound();
            }

            return Ok(Mapper.Map<IEnumerable<PoiDto>>(mCityRepo.GetPois(cityId)));
        }


        [HttpGet("{cityId}/poi/{id}", Name = "GetPoi")]
        public IActionResult GetPoi(int cityId, int id)
        {
            var poi = mCityRepo.GetPoi(cityId, id);
            if (poi == null) return NotFound();

            return Ok(Mapper.Map<PoiDto>(poi));
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

            if (!mCityRepo.DoesCityExist(cityId))
            {
                return NotFound(new { CityId = cityId });
            }

            var inputPoi = Mapper.Map<Poi>(poi);
            this.mCityRepo.AddPoi(cityId, inputPoi);

            if (! mCityRepo.Save())
            {
                return StatusCode(500, "Problem occured while saving to the database.");
            }

            var createdPoi = Mapper.Map<PoiDto>(inputPoi);
            return CreatedAtRoute("GetPoi", new { cityId = cityId, id = createdPoi.Id }, createdPoi);
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

            if (!mCityRepo.DoesCityExist(cityId))
            {
                return NotFound(new { CityId = cityId });
            }

            var existingPoi = mCityRepo.GetPoi(cityId, id);
            if (existingPoi == null)
            {
                return NotFound(new { PoiId = id });
            }

            Mapper.Map(poi, existingPoi);

            if (!mCityRepo.Save())
            {
                return StatusCode(500, "Problem occured while saving to the database.");
            }

            return NoContent();
        }


        [HttpPatch("{cityId}/poi/{id}")]
        public IActionResult PatchPoi(int cityId, int id, [FromBody] JsonPatchDocument<PoiUpdateDto> patch)
        {
            if (patch == null)
            {
                return BadRequest();
            }

            if (!mCityRepo.DoesCityExist(cityId))
            {
                return NotFound(new { CityId = cityId });
            }

            var existingPoi = mCityRepo.GetPoi(cityId, id);
            if (existingPoi == null)
            {
                return NotFound(new { PoiId = id });
            }

            var patched = Mapper.Map<PoiUpdateDto>(existingPoi);
            patch.ApplyTo(patched, ModelState);

            TryValidateModel(patched);
            if (! ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(patched, existingPoi);

            if (!mCityRepo.Save())
            {
                return StatusCode(500, "Problem occured while saving to the database.");
            }

            return NoContent();
        }


        [HttpDelete("{cityId}/poi/{id}")]
        public IActionResult DeletePoi(int cityId, int id)
        {
            if (!mCityRepo.DoesCityExist(cityId))
            {
                return NotFound(new { CityId = cityId });
            }

            var existingPoi = mCityRepo.GetPoi(cityId, id);
            if (existingPoi == null)
            {
                return NotFound(new { PoiId = id });
            }

            mCityRepo.DeletePoi(existingPoi);

            if (!mCityRepo.Save())
            {
                return StatusCode(500, "Problem occured while saving to the database.");
            }

            mSender.Send("POI deleted", $"POI {existingPoi.Id} '{existingPoi.Name}' was deleted.");

            return NoContent();
        }

        #endregion
    }
}
