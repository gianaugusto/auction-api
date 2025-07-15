using AutoAuction.Domain;
using AutoAuction.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAuction.Infrastructure
{
    public class DatabaseSeeder
    {
        private readonly DbContextOptions<DefaultContext> _options;

        public DatabaseSeeder(DbContextOptions<DefaultContext> options)
        {
            _options = options;
        }

        public async Task SeedAsync()
        {
            using var context = new DefaultContext(_options);

            if (!context.Vehicles.Any())
            {
                Vehicle[] vehicles = new Vehicle[]
                {
                    new Sedan("1", "Toyota", "Camry", 2020, 10000, 4),
                    new Sedan("2", "Honda", "Civic", 2019, 8000, 4),
                    new SUV("3", "Ford", "Explorer", 2021, 15000, 7)
                };

                await context.Vehicles.AddRangeAsync(vehicles);
                await context.SaveChangesAsync();
            }
        }
    }
}
