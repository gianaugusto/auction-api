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
            var name = "Test Auction";
            var startingBid = 100m;

            // Act
            var auction = service.CreateAuction(name, startingBid);

            // Assert
            Assert.Equal(name, auction.Name);
            Assert.Equal(startingBid, auction.StartingBid);
            Assert.Equal(startingBid, auction.CurrentBid);
            Assert.True(auction.IsActive);
        }
    }
}
