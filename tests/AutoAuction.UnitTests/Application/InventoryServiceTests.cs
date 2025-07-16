using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoAuction.Application;
using AutoAuction.Application.DTOs;
using AutoAuction.Domain;
using AutoAuction.Domain.Repositories;
using AutoAuction.UnitTests.Common;
using Moq;
using Xunit;

namespace AutoAuction.UnitTests.Application
{
    public class InventoryServiceTests
    {
        private readonly Mock<IInventoryRepository> _inventoryRepositoryMock;
        private readonly InventoryService _inventoryService;

        public InventoryServiceTests()
        {
            _inventoryRepositoryMock = new Mock<IInventoryRepository>();
            _inventoryService = new InventoryService(_inventoryRepositoryMock.Object);
        }

        [Fact]
        public async Task AddVehicleAsync_ShouldAddVehicle()
        {
            // Arrange
            var vehicleDto = Fixtures.Create<VehicleDto>();

            // Act
            await _inventoryService.AddVehicleAsync(vehicleDto, TestContext.Current.CancellationToken);

            // Assert
            _inventoryRepositoryMock.Verify(x => x.AddVehicleAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddVehicleAsync_ShouldThrowException_WhenVehicleDtoIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _inventoryService.AddVehicleAsync(null, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task GetVehicleByIdAsync_ShouldReturnVehicle()
        {
            // Arrange
            var vehicle = Fixtures.GetHatchback();
            _inventoryRepositoryMock.Setup(x => x.GetVehiclesByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(vehicle);

            // Act
            var result = await _inventoryService.GetVehicleByIdAsync(vehicle.Id, TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(vehicle, result);
            _inventoryRepositoryMock.Verify(x => x.GetVehiclesByIdAsync(vehicle.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchVehiclesAsync_ShouldReturnMatchingVehicles()
        {
            // Arrange
            var vehicles = new List<Vehicle>
            {
                Fixtures.GetHatchback(),
                Fixtures.GetHatchback()
            };
            _inventoryRepositoryMock.Setup(x => x.GetVehiclesAsync("Hatchback", null, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(vehicles);

            // Act
            var result = await _inventoryService.SearchVehiclesAsync("Hatchback", cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(2, result.Count());
            _inventoryRepositoryMock.Verify(x => x.GetVehiclesAsync("Hatchback", null, null, null, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchVehiclesAsync_ShouldThrowException_WhenYearIsInvalid()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _inventoryService.SearchVehiclesAsync(year: 0, cancellationToken: TestContext.Current.CancellationToken));
        }
    }
}
