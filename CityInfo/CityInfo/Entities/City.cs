using CityInfo.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.Entities
{
    public class City
    {
        /// <summary>
        /// Id and CityId are automatically set as primary key.
        /// Such properties, when int or Guid, are auto-generated (identity).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(Constants.MaxNameLength)]
        public string Name { get; set; }

        [MaxLength(Constants.MaxDescriptionLength)]
        public string Description { get; set; }

        public ICollection<Poi> Poi { get; set; } = new List<Poi>();
    }
}
