using System;
using AutoAuction.Application.DTOs;
using AutoAuction.Application.Mappers;
using AutoAuction.Domain;
using AutoAuction.UnitTests.Common;
using Xunit;

namespace AutoAuction.UnitTests.Application.Mappers
{
    public class VehicleMapperTests
    {
        [Fact]
        public void ToDomain_ShouldMapVehicleDtoToHatchback()
        {
            // Arrange
            var vehicleDto = Fixtures.Create<VehicleDto>();
            vehicleDto.Type = VehicleType.Hatchback;
            vehicleDto.NumberOfDoors = 5;

            // Act
            var vehicle = vehicleDto.ToDomain();

            // Assert
            Assert.IsType<Hatchback>(vehicle);
            Assert.Equal(vehicleDto.Id, vehicle.Id);
            Assert.Equal(vehicleDto.Manufacturer, vehicle.Manufacturer);
            Assert.Equal(vehicleDto.Model, vehicle.Model);
            Assert.Equal(vehicleDto.Year, vehicle.Year);
            Assert.Equal(vehicleDto.StartingBid, vehicle.StartingBid);
            Assert.Equal(vehicleDto.NumberOfDoors, ((Hatchback)vehicle).NumberOfDoors);
        }

        [Fact]
        public void ToDomain_ShouldMapVehicleDtoToSedan()
        {
            // Arrange
            var vehicleDto = Fixtures.Create<VehicleDto>();
            vehicleDto.Type = VehicleType.Sedan;
            vehicleDto.NumberOfDoors = 4;

            // Act
            var vehicle = vehicleDto.ToDomain();

            // Assert
            Assert.IsType<Sedan>(vehicle);
            Assert.Equal(vehicleDto.Id, vehicle.Id);
            Assert.Equal(vehicleDto.Manufacturer, vehicle.Manufacturer);
            Assert.Equal(vehicleDto.Model, vehicle.Model);
            Assert.Equal(vehicleDto.Year, vehicle.Year);
            Assert.Equal(vehicleDto.StartingBid, vehicle.StartingBid);
            Assert.Equal(vehicleDto.NumberOfDoors, ((Sedan)vehicle).NumberOfDoors);
        }

        [Fact]
        public void ToDomain_ShouldMapVehicleDtoToSUV()
        {
            // Arrange
            var vehicleDto = Fixtures.Create<VehicleDto>();
            vehicleDto.Type = VehicleType.SUV;
            vehicleDto.NumberOfSeats = 7;

            // Act
            var vehicle = vehicleDto.ToDomain();

            // Assert
            Assert.IsType<SUV>(vehicle);
            Assert.Equal(vehicleDto.Id, vehicle.Id);
            Assert.Equal(vehicleDto.Manufacturer, vehicle.Manufacturer);
            Assert.Equal(vehicleDto.Model, vehicle.Model);
            Assert.Equal(vehicleDto.Year, vehicle.Year);
            Assert.Equal(vehicleDto.StartingBid, vehicle.StartingBid);
            Assert.Equal(vehicleDto.NumberOfSeats, ((SUV)vehicle).NumberOfSeats);
        }

        [Fact]
        public void ToDomain_ShouldMapVehicleDtoToTruck()
        {
            // Arrange
            var vehicleDto = Fixtures.Create<VehicleDto>();
            vehicleDto.Type = VehicleType.Truck;
            vehicleDto.LoadCapacity = 1000m;

            // Act
            var vehicle = vehicleDto.ToDomain();

            // Assert
            Assert.IsType<Truck>(vehicle);
            Assert.Equal(vehicleDto.Id, vehicle.Id);
            Assert.Equal(vehicleDto.Manufacturer, vehicle.Manufacturer);
            Assert.Equal(vehicleDto.Model, vehicle.Model);
            Assert.Equal(vehicleDto.Year, vehicle.Year);
            Assert.Equal(vehicleDto.StartingBid, vehicle.StartingBid);
            Assert.Equal(vehicleDto.LoadCapacity, ((Truck)vehicle).LoadCapacity);
        }

        [Fact]
        public void ToDomain_ShouldThrowException_WhenVehicleDtoIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ((VehicleDto)null).ToDomain());
        }

        [Fact]
        public void ToDomain_ShouldThrowException_WhenVehicleTypeIsInvalid()
        {
            // Arrange
            var vehicleDto = Fixtures.Create<VehicleDto>();
            vehicleDto.Type = (VehicleType)999; // Invalid type

            // Act & Assert
            Assert.Throws<ArgumentException>(() => vehicleDto.ToDomain());
        }
    }
}
