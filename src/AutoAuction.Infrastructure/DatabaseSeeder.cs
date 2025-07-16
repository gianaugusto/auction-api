using AutoAuction.Domain;
using AutoAuction.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAuction.Infrastructure
{
    public class DatabaseSeeder
    {
        private readonly DefaultContext _context;

        public DatabaseSeeder(DefaultContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (!_context.Vehicles.Any())
            {
                Vehicle[] vehicles = new Vehicle[]
                {
                new Sedan("1", "Toyota", "Camry", 2020, 10000, 4),
                new Sedan("2", "Honda", "Civic", 2019, 8000, 4),
                new SUV("3", "Ford", "Explorer", 2021, 15000, 7)
                };

                await _context.Vehicles.AddRangeAsync(vehicles);
                await _context.SaveChangesAsync();
            }
        }
    }

}
