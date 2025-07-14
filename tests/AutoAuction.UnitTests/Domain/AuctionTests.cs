using AutoAuction.Domain;
using Xunit;

namespace AutoAuction.UnitTests.Domain
{
    public class AuctionTests
    {
        [Fact]
        public void CreateAuction_ShouldInitializeWithStartingBid()
        {
            // Arrange
            var name = "Test Auction";
            var startingBid = 100m;

            // Act
            var auction = new Auction
            {
                Name = name,
                StartingBid = startingBid,
                CurrentBid = startingBid,
                IsActive = true
            };

            // Assert
            Assert.Equal(name, auction.Name);
            Assert.Equal(startingBid, auction.StartingBid);
            Assert.Equal(startingBid, auction.CurrentBid);
            Assert.True(auction.IsActive);
        }
    }
}
