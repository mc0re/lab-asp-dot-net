using Microsoft.AspNetCore.Mvc;
using System;

namespace SwaggerLab
{
    /// <summary>
    /// APIs regarding the hero name.
    /// </summary>
    [ApiController]
    [Route("/api/heroes")]
    [Produces("application/json")]
    public class HeroesController : ControllerBase
    {
        /// <summary>
        /// Default GET API just to not show error 404.
        /// </summary>
        [HttpGet]
        public IActionResult GetDefault()
        {
            return Redirect("/api/heroes/name,last");
        }


        /// <summary>
        /// API to generate hero name from the first and last names of the person
        /// and return it as <see cref="Person"/>.
        /// </summary>
        /// <param name="firstName">Real first name</param>
        /// <param name="lastName">Real last name</param>
        /// <returns>An instance of the hero's <see cref="Person"/> class</returns>
        [HttpGet("{firstName},{lastName}")]
        [ProducesResponseType(200, Type = typeof(Person))]
        [ProducesResponseType(404)]
        public IActionResult GetFromName(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentNullException(nameof(firstName));

            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentNullException(nameof(lastName));

            var p = new Person() { FirstName = firstName, LastName = lastName };
            p.SetHeroName();
            return Ok(p);
        }


        /// <summary>
        /// Dummy creation of a new hero person.
        /// </summary>
        /// <param name="per">Hero definition as <see cref="Person"/></param>
        [HttpPost]
        [ProducesResponseType(201)]
        public IActionResult Create([FromBody] Person per)
        {
            return this.Created("Get", per);
        }
    }
}
