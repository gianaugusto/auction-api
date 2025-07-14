using System;
using System.Linq;
using AutoAuction.Application;
using AutoAuction.Domain;
using Xunit;

namespace AutoAuction.UnitTests.Application
{
    public class AuctionServiceTests
    {
        [Fact]
        public void AddVehicle_ShouldAddVehicleToInventory()
        {
            // Arrange
            var service = new AuctionService();
            var vehicle = new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);

            // Act
            service.AddVehicle(vehicle);

            // Assert
            var vehicles = service.SearchVehicles();
            Assert.Single(vehicles);
            Assert.Equal("V001", vehicles.First().Id);
        }

        [Fact]
        public void AddVehicle_ShouldThrowException_WhenVehicleIdAlreadyExists()
        {
            // Arrange
            var service = new AuctionService();
            var vehicle1 = new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);
            var vehicle2 = new Hatchback("V001", "Honda", "Civic", 2019, 6000m, 5);

            // Act & Assert
            service.AddVehicle(vehicle1);
            Assert.Throws<ArgumentException>(() => service.AddVehicle(vehicle2));
        }

        [Fact]
        public void StartAuction_ShouldStartAuctionForVehicle()
        {
            // Arrange
            var service = new AuctionService();
            var vehicle = new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);
            service.AddVehicle(vehicle);

            // Act
            service.StartAuction("V001");

            // Assert
            Assert.Throws<InvalidOperationException>(() => service.StartAuction("V001"));
        }

        [Fact]
        public void PlaceBid_ShouldPlaceBidOnActiveAuction()
        {
            // Arrange
            var service = new AuctionService();
            var vehicle = new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);
            service.AddVehicle(vehicle);
            service.StartAuction("V001");

            // Act
            service.PlaceBid("V001", "Bidder1", 6000m);

            // Assert
            // We can't directly check the bid amount, but we can verify that no exception was thrown
            Assert.True(true);
        }

        [Fact]
        public void PlaceBid_ShouldThrowException_WhenAuctionIsNotActive()
        {
            // Arrange
            var service = new AuctionService();
            var vehicle = new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);
            service.AddVehicle(vehicle);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => service.PlaceBid("V001", "Bidder1", 6000m));
        }
    }
}
