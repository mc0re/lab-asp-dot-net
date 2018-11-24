using CityInfo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Entities
{
    public class Poi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required, MaxLength(Constants.MaxNameLength)]
        public string Name { get; set; }

        [MaxLength(Constants.MaxDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// This is made into a navigation by convention, with a foreign key,
        /// so [ForeignKey] is not needed.
        /// </summary>
        [ForeignKey(nameof(CityId))]
        public City City { get; set; }


        /// <summary>
        /// This (class name + Id) is made into a foreign key by convention.
        /// </summary>
        public int CityId { get; set; }
    }
}
