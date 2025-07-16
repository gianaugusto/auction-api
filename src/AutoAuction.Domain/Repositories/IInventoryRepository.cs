using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoAuction.Domain;

namespace AutoAuction.Domain.Repositories
{
    public interface IInventoryRepository
    {
        Task AddVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
        
        Task<IEnumerable<Vehicle>> GetVehiclesAsync(string type = null, string manufacturer = null, string model = null, int? year = null, CancellationToken cancellationToken = default);
        
        Task<Vehicle> GetVehiclesByIdAsync(string vehicleId, CancellationToken cancellationToken);
    }
}
