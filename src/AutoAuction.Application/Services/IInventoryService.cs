using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoAuction.Application.DTOs;
using AutoAuction.Domain;

namespace AutoAuction.Application
{
    public interface IInventoryService
    {
        Task AddVehicleAsync(VehicleDto vehicleDto, CancellationToken cancellationToken = default);

        public Task<Vehicle> GetVehicleByIdAsync(string vehicleId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Vehicle>> SearchVehiclesAsync(string type = null, string manufacturer = null, string model = null, int? year = null, CancellationToken cancellationToken = default);
    }
}
