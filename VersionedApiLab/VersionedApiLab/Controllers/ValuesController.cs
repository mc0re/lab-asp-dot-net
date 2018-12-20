using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace VersionedApiLab.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [ApiController]
    [Route("api/values")]
    public class ValuesController : ControllerBase
    {
        [MapToApiVersion("1.0")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Version 1.0", "value-1", "value-2", "value-3" };
        }


        [MapToApiVersion("1.1")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetWithUnderscores()
        {
            return new string[] { "Version 1.1", "value_1", "value_2", "value_3" };
        }
    }


    [ApiVersion("1.2")]
    [ApiController]
    [Route("api/values")]
    public class ValuesV12Controller : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Version 1.2", "value1", "value2", "value3" };
        }
    }
}
