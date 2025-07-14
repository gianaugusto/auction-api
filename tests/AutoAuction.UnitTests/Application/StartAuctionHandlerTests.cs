using System;
using AutoAuction.Application;
using AutoAuction.Domain;
using AutoAuction.Domain.Repositories;
using Moq;
using Xunit;

namespace AutoAuction.UnitTests.Application
{
    public class StartAuctionHandlerTests
    {
        [Fact]
        public void Handle_ShouldStartAuctionForVehicle()
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
    }
}
