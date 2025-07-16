using AutoAuction.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;

namespace AutoAuction.Infrastructure
{
    public class IntegrationTestSeeder
    {
        private readonly DefaultContext _context;
        private readonly IFixture _fixture;

        public IntegrationTestSeeder(DefaultContext context)
        {
            _context = context;
            _fixture = new Fixture();
            _fixture.Customizations.Add(
                new AutoFixture.Kernel.TypeRelay(
                    typeof(Vehicle),
                    typeof(Hatchback)));

            _fixture.Customizations.Add(
                new AutoFixture.Kernel.TypeRelay(
                    typeof(Vehicle),
                    typeof(Sedan)));

            _fixture.Customizations.Add(
                new AutoFixture.Kernel.TypeRelay(
                    typeof(Vehicle),
                    typeof(SUV)));

            _fixture.Customizations.Add(
                new AutoFixture.Kernel.TypeRelay(
                    typeof(Vehicle),
                    typeof(Truck)));
            
            _fixture.Register(() =>
                new Truck(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                2020, // Set a valid year
                5000m, // Set a valid starting bid
                5 // Set a valid number of doors
                )
            );

            _fixture.Register(() =>
                new SUV(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                2020, // Set a valid year
                5000m, // Set a valid starting bid
                5 // Set a valid number of doors
                )
            );

            _fixture.Register(() =>
                new Sedan(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                2020, // Set a valid year
                5000m, // Set a valid starting bid
                5 // Set a valid number of doors
                )
            );

            _fixture.Register(() =>
                new Hatchback(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                2020, // Set a valid year
                5000m, // Set a valid starting bid
                5 // Set a valid number of doors
                )
            );
                
            _context.Database.EnsureCreated();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (!_context.Vehicles.Any())
            {
                await AddVehiclesAsync();
            }

            if (!_context.Auctions.Any())
            {
                await AddAuctionsAsync();
            }
        }

        public async Task<IEnumerable<Vehicle>> AddVehiclesAsync(int numberOfVehiclea = 1)
        {
            var vehicles = _fixture.CreateMany<Vehicle>(numberOfVehiclea);
            await _context.Vehicles.AddRangeAsync(vehicles);
            await _context.SaveChangesAsync();

            return vehicles;
        }

        public async Task<Vehicle> AddVehicleAsync()
        {
            var vehicle = _fixture.Create<Vehicle>();
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();

            return vehicle;
        }

        public async Task AddAuctionsAsync()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            var auctions = new List<Auction>();
            foreach (var vehicle in vehicles)
            {
                var auction = new Auction(vehicle);
                auction.StartAuction();

                auctions.Add(auction);
            }
            await _context.Auctions.AddRangeAsync(auctions);
            await _context.SaveChangesAsync();
        }

        public async Task<Auction> GetAnyActiveAuctionsAsync()
        {
            return await _context.Auctions.Where(o => o.IsActive == true).FirstAsync();
        }

        public async Task<Auction> GetAuctionsByIdAsync(int id)
        {
            return await _context.Auctions
                    .AsNoTracking()
                    .Include(a => a.Vehicle)
                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task RemoveVehiclesAsync()
        {
            _context.Vehicles.RemoveRange(_context.Vehicles);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAuctionsAsync()
        {
            _context.Auctions.RemoveRange(_context.Auctions);
            await _context.SaveChangesAsync();
        }
    }
}
