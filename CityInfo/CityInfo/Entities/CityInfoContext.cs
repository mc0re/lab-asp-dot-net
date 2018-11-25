using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(new City
            {
                Id = 1,
                Name = "New York City",
                Description = "The one with that big park."
            });
            modelBuilder.Entity<Poi>().HasData(new Poi { CityId = 1, Id = 1, Name = "Central Park", Description = "The most visited urban park in the United States." });
            modelBuilder.Entity<Poi>().HasData(new Poi { CityId = 1, Id = 2, Name = "Empire State Building", Description = "A 102-story skyscraper located in Midtown Manhattan." });

            modelBuilder.Entity<City>().HasData(new City
            {
                Id = 2,
                Name = "Antwerp",
                Description = "The one with the cathdral that was never really finished."
            });
            modelBuilder.Entity<Poi>().HasData(new Poi { CityId = 2, Id = 3, Name = "Cathedral of Our Lady", Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans." });
            modelBuilder.Entity<Poi>().HasData(new Poi { CityId = 2, Id = 4, Name = "Antwerp Central Station", Description = "The finest example of railway architecture in Belgium." });

            modelBuilder.Entity<City>().HasData(new City
            {
                Id = 3,
                Name = "Paris",
                Description = "The one with that big tower."
            });
            modelBuilder.Entity<Poi>().HasData(new Poi { CityId = 3, Id = 5, Name = "Eiffel Tower", Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel." });
            modelBuilder.Entity<Poi>().HasData(new Poi { CityId = 3, Id = 6, Name = "The Louvre", Description = "The world's largest museum." });

            base.OnModelCreating(modelBuilder);
        }
    }
}
