using System.ComponentModel.DataAnnotations;


namespace SwaggerLab
{
    /// <summary>
    /// Person object to represent a team member.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Name of the person.
        /// </summary>
        [Required]
        public string FirstName { get; set; }


        /// <summary>
        /// Surname of the person.
        /// </summary>
        [Required]
        public string LastName { get; set; }


        /// <summary>
        /// Generated superhero name.
        /// </summary>
        public string HeroName { get; private set; }


        /// <summary>
        /// Call name generator, the name appears in <see cref="HeroName"/>.
        /// </summary>
        public void SetHeroName()
        {
            HeroName = HeroGenerator.GetHeroName(FirstName, LastName);
        }
    }
}
