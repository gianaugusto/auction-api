using System;
using AutoAuction.Application;
using AutoAuction.Application.DTOs;
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
            var vehicleDto = new VehicleDto
            {
                Id = "V001",
                Type = "Hatchback",
                Manufacturer = "Toyota",
                Model = "Yaris",
                Year = 2020,
                StartingBid = 5000m,
                NumberOfDoors = 5
            };
            service.AddVehicle(vehicleDto);

            // Act
            service.StartAuction("V001");

            // Assert
            mockRepository.Verify(r => r.AddAuction(It.IsAny<Auction>()), Times.Once);
        }
    }
}
