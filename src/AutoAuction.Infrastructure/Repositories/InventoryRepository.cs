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
    public class InventoryRepository(DefaultContext context) : IInventoryRepository
    {
        private readonly DefaultContext _context = context;

        public async Task AddVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle));

            if (await _context.Vehicles.AnyAsync(v => v.Id == vehicle.Id, cancellationToken))
                throw new ArgumentException("Vehicle with this ID already exists", nameof(vehicle));

            await _context.Vehicles.AddAsync(vehicle, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync(string type = null, string manufacturer = null, string model = null, int? year = null, CancellationToken cancellationToken = default)
        {
            if (year != null && year <= 0)
                throw new ArgumentOutOfRangeException(nameof(year), "Year must be greater than zero");

            var query = _context.Vehicles.AsQueryable();

            if (!string.IsNullOrEmpty(type))
                query = query.Where(v => EF.Property<string>(v, "VehicleType").Equals(type, StringComparison.OrdinalIgnoreCase));

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
            return await _context.Vehicles.FindAsync(vehicleId, cancellationToken);
        }
    }
}
