using AutoAuction.Application.DTOs;
using AutoAuction.Application.Mappers;
using AutoAuction.Domain;
using AutoAuction.Domain.Exceptions;
using AutoAuction.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AutoAuction.Application
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
        }

        public async Task AddVehicleAsync(VehicleDto vehicleDto, CancellationToken cancellationToken = default)
        {
            if (vehicleDto == null)
                throw new ArgumentNullException(nameof(vehicleDto));

            Vehicle vehicle = vehicleDto.ToDomain();

            await _inventoryRepository.AddVehicleAsync(vehicle, cancellationToken);
        }

        public Task<Vehicle> GetVehicleByIdAsync(string vehicleId, CancellationToken cancellationToken = default)
        {
            return _inventoryRepository.GetVehiclesByIdAsync(vehicleId, cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> SearchVehiclesAsync(string type = null, string manufacturer = null, string model = null, int? year = null, CancellationToken cancellationToken = default)
        {
            if (year != null && year <= 0)
                throw new ArgumentOutOfRangeException(nameof(year), "Year must be greater than zero");

            var vehicles = await _inventoryRepository.GetVehiclesAsync(type, manufacturer, model, year, cancellationToken);

            return vehicles;
        }
    }
}
