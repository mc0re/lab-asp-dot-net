using System.ComponentModel.DataAnnotations;

namespace CityInfo.Models
{
    public class PoiNewDto
    {
        [Required(ErrorMessage = "Please provide a name")]
        [MaxLength(Constants.MaxNameLength)]
        public string Name { get; set; }


        [MaxLength(Constants.MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
