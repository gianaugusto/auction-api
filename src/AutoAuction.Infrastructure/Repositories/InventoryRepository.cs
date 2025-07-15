using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoAuction.Domain;
using AutoAuction.Domain.Repositories;
using AutoAuction.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AutoAuction.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DbContextOptions<DefaultContext> _options;

        public InventoryRepository(DbContextOptions<DefaultContext> options)
        {
            _options = options;
        }

        public async Task AddVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle));

            using var context = new DefaultContext(_options);
            if (await context.Vehicles.AnyAsync(v => v.Id == vehicle.Id, cancellationToken))
                throw new ArgumentException("Vehicle with this ID already exists", nameof(vehicle));

            await context.Vehicles.AddAsync(vehicle, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync(string type = null, string manufacturer = null, string model = null, int? year = null, CancellationToken cancellationToken = default)
        {
            if (year != null && year <= 0)
                throw new ArgumentOutOfRangeException(nameof(year), "Year must be greater than zero");

            using var context = new DefaultContext(_options);
            var query = context.Vehicles.AsQueryable();

            if (!string.IsNullOrEmpty(type))
                query = query.Where(v => v.GetType().Name.Equals(type, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(manufacturer))
                query = query.Where(v => v.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(model))
                query = query.Where(v => v.Model.Equals(model, StringComparison.OrdinalIgnoreCase));

            if (year.HasValue)
                query = query.Where(v => v.Year == year.Value);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Vehicle> GetVehiclesByIdAsync(string vehicleId, CancellationToken cancellationToken)
        {
            using var context = new DefaultContext(_options);
            return await context.Vehicles.FindAsync(vehicleId, cancellationToken);
        }
    }
}
