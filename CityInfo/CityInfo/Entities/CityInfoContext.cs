using Microsoft.EntityFrameworkCore;

namespace CityInfo.Entities
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; }

        public DbSet<Poi> Pois { get; set; }

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}
