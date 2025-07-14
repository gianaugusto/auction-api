using AutoAuction.Application;
using AutoAuction.Domain;
using Xunit;

namespace AutoAuction.UnitTests.Application
{
    public class StartAuctionHandlerTests
    {
        [Fact]
        public void CreateAuction_ShouldInitializeWithStartingBid()
        {
            // Arrange
            var service = new AuctionService();
            var vehicle = new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);
            service.AddVehicle(vehicle);

            // Act
            service.StartAuction(vehicle.Id);

            // Assert
            Assert.True(service.GetActiveAuctions().ContainsKey(vehicle.Id));
            var auction = service.GetActiveAuctions()[vehicle.Id];
            Assert.Equal(vehicle.StartingBid, auction.CurrentHighestBid);
            Assert.True(auction.IsActive);
        }
    }
}
