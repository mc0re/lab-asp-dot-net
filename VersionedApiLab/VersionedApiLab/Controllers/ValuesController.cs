using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VersionedApiLab.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Version 1.0", "value-1", "value-2", "value-3" };
        }
    }


    [ApiVersion("1.1")]
    [Route("api/values")]
    [ApiController]
    public class ValuesV11Controller : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Version 1.1", "value-1", "value-2", "value-3" };
        }
    }
}
