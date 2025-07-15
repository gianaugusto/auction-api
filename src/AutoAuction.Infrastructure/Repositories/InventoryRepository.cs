using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoAuction.Domain;
using AutoAuction.Domain.Repositories;

namespace AutoAuction.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ConcurrentBag<Vehicle> _vehicles = new();

        public Task AddVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle));

            if (_vehicles.Any(v => v.Id == vehicle.Id))
                throw new ArgumentException("Vehicle with this ID already exists", nameof(vehicle));

            _vehicles.Add(vehicle);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Vehicle>> GetVehiclesAsync(string type = null, string manufacturer = null, string model = null, int? year = null, CancellationToken cancellationToken = default)
        {
            if (year != null && year <= 0)
                throw new ArgumentOutOfRangeException(nameof(year), "Year must be greater than zero");

            var vehicles = _vehicles.Where(v =>
                (type == null || v.GetType().Name.Equals(type, System.StringComparison.OrdinalIgnoreCase)) &&
                (manufacturer == null || v.Manufacturer.Equals(manufacturer, System.StringComparison.OrdinalIgnoreCase)) &&
                (model == null || v.Model.Equals(model, System.StringComparison.OrdinalIgnoreCase)) &&
                (year == null || v.Year == year)
            );
            return Task.FromResult(vehicles);
        }

        public Task<Vehicle> GetVehiclesByIdAsync(string vehicleId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_vehicles.FirstOrDefault(o => o.Id==vehicleId));
        }
    }
}
