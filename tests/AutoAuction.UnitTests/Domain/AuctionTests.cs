using System;
using AutoAuction.Domain;
using AutoAuction.Domain.Exceptions;
using AutoAuction.UnitTests.Common;
using Xunit;

namespace AutoAuction.UnitTests.Domain
{
    public class AuctionTests
    {
        [Fact(Skip ="TBR")]
        public void Auction_ShouldBeCreatedWithValidVehicle()
        {
            // Arrange
            var vehicle = Fixtures.GetHatchback();

            // Act
            var auction = new Auction(vehicle);

            // Assert
            Assert.Equal(1, auction.Id);
            Assert.Equal(vehicle, auction.Vehicle);
            Assert.False(auction.IsActive);
            Assert.Equal(vehicle.StartingBid, auction.CurrentHighestBid);
        }

        [Fact]
        public void Auction_ShouldThrowException_WhenVehicleIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Auction(null));
        }

        [Fact]
        public void StartAuction_ShouldActivateAuction()
        {
            // Arrange
            var auction = new Auction(Fixtures.GetHatchback());

            // Act
            auction.StartAuction();

            // Assert
            Assert.True(auction.IsActive);
        }

        [Fact]
        public void StartAuction_ShouldThrowException_WhenAuctionIsAlreadyActive()
        {
            // Arrange
            var auction = new Auction(Fixtures.GetHatchback());
            auction.StartAuction();

            // Act & Assert
            Assert.Throws<AuctionAlreadyActiveException>(() => auction.StartAuction());
        }

        [Fact]
        public void PlaceBid_ShouldUpdateCurrentHighestBid()
        {
            // Arrange
            var auction = new Auction(Fixtures.GetHatchback());
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
            var auction = new Auction(Fixtures.GetHatchback());

            // Act & Assert
            Assert.Throws<AuctionNotActiveException>(() => auction.PlaceBid("Bidder1", 6000m));
        }

        [Fact]
        public void PlaceBid_ShouldThrowException_WhenBidderIdIsNullOrEmpty()
        {
            // Arrange
            var auction = new Auction(Fixtures.GetHatchback());
            auction.StartAuction();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => auction.PlaceBid(string.Empty, 6000m));
        }

        [Fact(Skip ="TBR")]
        public void PlaceBid_ShouldThrowException_WhenBidAmountIsInvalid()
        {
            // Arrange
            var auction = new Auction(Fixtures.GetHatchback());
            auction.StartAuction();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => auction.PlaceBid("Bidder1", 0m));
        }

        [Fact]
        public void PlaceBid_ShouldThrowException_WhenBidAmountIsLessThanCurrentHighestBid()
        {
            // Arrange
            var auction = new Auction(Fixtures.GetHatchback());
            auction.StartAuction();

            // Act & Assert
            Assert.Throws<InvalidBidAmountException>(() => auction.PlaceBid("Bidder1", 4000m));
        }

        [Fact]
        public void CloseAuction_ShouldDeactivateAuction()
        {
            // Arrange
            var auction = new Auction(Fixtures.GetHatchback());
            auction.StartAuction();

            // Act
            auction.CloseAuction();

            // Assert
            Assert.False(auction.IsActive);
        }

        [Fact]
        public void CloseAuction_ShouldThrowException_WhenAuctionIsNotActive()
        {
            // Arrange
            var auction = new Auction(Fixtures.GetHatchback());

            // Act & Assert
            Assert.Throws<AuctionNotActiveException>(() => auction.CloseAuction());
        }
    }
}
