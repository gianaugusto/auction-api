using System;
using AutoAuction.Domain;
using AutoAuction.Domain.Exceptions;
using Xunit;

namespace AutoAuction.UnitTests.Domain
{
    public class AuctionTests
    {
        [Fact]
        public void StartAuction_ShouldSetIsActiveToTrue()
        {
            // Arrange
            var vehicle = new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);
            var auction = new Auction(vehicle);

            // Act
            auction.StartAuction();

            // Assert
            Assert.True(auction.IsActive);
        }

        [Fact]
        public void PlaceBid_ShouldIncreaseCurrentHighestBid()
        {
            // Arrange
            var vehicle = new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);
            var auction = new Auction(vehicle);
            auction.StartAuction();

            // Act
            auction.PlaceBid("Bidder1", 6000m);

            // Assert
            Assert.Equal(6000m, auction.CurrentHighestBid);
        }

        [Fact]
        public void PlaceBid_ShouldThrowException_WhenAuctionIsNotActive()
        {
            // Arrange
            var vehicle = new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);
            var auction = new Auction(vehicle);

            // Act & Assert
            Assert.Throws<AuctionNotActiveException>(() => auction.PlaceBid("Bidder1", 6000m));
        }

        [Fact]
        public void PlaceBid_ShouldThrowException_WhenBidAmountIsInvalid()
        {
            // Arrange
            var vehicle = new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);
            var auction = new Auction(vehicle);
            auction.StartAuction();

            // Act & Assert
            Assert.Throws<InvalidBidAmountException>(() => auction.PlaceBid("Bidder1", 4000m));
        }
    }
}
