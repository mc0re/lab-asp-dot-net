using System.Collections.Generic;

namespace CityInfo.Models
{
    public class CityDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<PoiDto> Poi { get; set; } = new List<PoiDto>();

        public int NofPoi
        {
            get { return Poi.Count; }
        }
    }
}
