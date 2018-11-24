using CityInfo.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Controllers
{
    [Route("api/createdb")]
    public class CreateDatabaseController : Controller
    {
        public CreateDatabaseController(CityInfoContext context)
        {
        }


        [HttpGet]
        public IActionResult CreateDatabase()
        {
            return Ok();
        }
    }
}
