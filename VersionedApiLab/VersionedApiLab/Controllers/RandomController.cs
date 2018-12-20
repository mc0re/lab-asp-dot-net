using System;
using Microsoft.AspNetCore.Mvc;

namespace VersionedApiLab
{
    [Route("api/random")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RandomController : Controller
    {
        private Random mRnd = new Random();


        [HttpGet]
        public IActionResult GetNumber()
        {
            return Ok(mRnd.Next(100));
        }


        [HttpGet]
        public IActionResult GetNumberAsObject()
        {
            return Ok(new { number = mRnd.Next(100) });
        }
    }
}
