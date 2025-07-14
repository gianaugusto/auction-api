using System;
using System.Linq;
using AutoAuction.Application;
using AutoAuction.Domain;
using AutoAuction.Domain.Repositories;
using Moq;
using Xunit;

namespace AutoAuction.UnitTests.Application
{
    public class AuctionServiceTests
    {
        [Fact]
        public void AddVehicle_ShouldAddVehicleToInventory()
        {
            // Arrange
            var mockRepository = new Mock<IAuctionRepository>();
            var service = new AuctionService(mockRepository.Object);
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
            var mockRepository = new Mock<IAuctionRepository>();
            var service = new AuctionService(mockRepository.Object);
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
            var mockRepository = new Mock<IAuctionRepository>();
            var service = new AuctionService(mockRepository.Object);
            var vehicle = new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);
            service.AddVehicle(vehicle);

            // Act
            service.StartAuction("V001");

            // Assert
            mockRepository.Verify(r => r.AddAuction(It.IsAny<Auction>()), Times.Once);
        }

        [Fact]
        public void PlaceBid_ShouldPlaceBidOnActiveAuction()
        {
            // Arrange
            var mockRepository = new Mock<IAuctionRepository>();
            var auction = new Auction(new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5));
            auction.StartAuction();
            mockRepository.Setup(r => r.GetAuctionById(1)).Returns(auction);

            var service = new AuctionService(mockRepository.Object);

            // Act
            service.PlaceBid(1, "Bidder1", 6000m);

            // Assert
            Assert.Equal(6000m, auction.CurrentHighestBid);
        }

        [Fact]
        public void PlaceBid_ShouldThrowException_WhenAuctionIsNotActive()
        {
            // Arrange
            var mockRepository = new Mock<IAuctionRepository>();
            var auction = new Auction(new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5));
            mockRepository.Setup(r => r.GetAuctionById(1)).Returns(auction);

            var service = new AuctionService(mockRepository.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => service.PlaceBid(1, "Bidder1", 6000m));
        }

        [Fact]
        public void PlaceBid_ShouldThrowException_WhenBidAmountIsInvalid()
        {
            // Arrange
            var mockRepository = new Mock<IAuctionRepository>();
            var auction = new Auction(new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5));
            auction.StartAuction();
            mockRepository.Setup(r => r.GetAuctionById(1)).Returns(auction);

            var service = new AuctionService(mockRepository.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.PlaceBid(1, "Bidder1", 4000m));
        }
    }
}
