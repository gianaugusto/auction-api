using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoAuction.Application;
using AutoAuction.Domain;
using AutoAuction.Domain.Exceptions;
using AutoAuction.Domain.Repositories;
using AutoAuction.UnitTests.Common;
using Moq;
using Xunit;

namespace AutoAuction.UnitTests.Application
{
    public class AuctionServiceTests
    {
        private readonly Mock<IAuctionRepository> _auctionRepositoryMock;
        private readonly Mock<IInventoryService> _inventoryServiceMock;
        private readonly AuctionService _auctionService;

        public AuctionServiceTests()
        {
            _auctionRepositoryMock = new Mock<IAuctionRepository>();
            _inventoryServiceMock = new Mock<IInventoryService>();
            _auctionService = new AuctionService(_auctionRepositoryMock.Object, _inventoryServiceMock.Object);
        }

        [Fact]
        public async Task StartAuctionAsync_ShouldStartAuctionForVehicle()
        {
            // Arrange
            var vehicleId = "V001";
            var vehicle = Fixtures.GetHatchback();
            _inventoryServiceMock.Setup(x => x.GetVehicleByIdAsync(vehicleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(vehicle);

            // Act
            await _auctionService.StartAuctionAsync(vehicleId, TestContext.Current.CancellationToken);

            // Assert
            _auctionRepositoryMock.Verify(x => x.StartAuctionForVehicleAsync(vehicle, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task StartAuctionAsync_ShouldThrowException_WhenVehicleIdIsNullOrEmpty()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _auctionService.StartAuctionAsync(string.Empty, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task StartAuctionAsync_ShouldThrowException_WhenVehicleNotFound()
        {
            // Arrange
            var vehicleId = "V001";
            _inventoryServiceMock.Setup(x => x.GetVehicleByIdAsync(vehicleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Vehicle)null);

            // Act & Assert
            await Assert.ThrowsAsync<VehicleNotFoundException>(() => _auctionService.StartAuctionAsync(vehicleId, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldPlaceBidOnActiveAuction()
        {
            // Arrange
            var auctionId = 1;
            var bidderId = "Bidder1";
            var bidAmount = 6000m;

            // Act
            await _auctionService.PlaceBidAsync(auctionId, bidderId, bidAmount, TestContext.Current.CancellationToken);

            // Assert
            _auctionRepositoryMock.Verify(x => x.PlaceBidAsync(auctionId, bidderId, bidAmount, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldThrowException_WhenAuctionIdIsInvalid()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _auctionService.PlaceBidAsync(0, "Bidder1", 6000m, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldThrowException_WhenBidderIdIsNullOrEmpty()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _auctionService.PlaceBidAsync(1, string.Empty, 6000m, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldThrowException_WhenBidAmountIsInvalid()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _auctionService.PlaceBidAsync(1, "Bidder1", 0m, TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task CloseAuctionAsync_ShouldCloseAuction()
        {
            // Arrange
            var auctionId = 1;

            // Act
            await _auctionService.CloseAuctionAsync(auctionId, TestContext.Current.CancellationToken);

            // Assert
            _auctionRepositoryMock.Verify(x => x.CloseAuctionAsync(auctionId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CloseAuctionAsync_ShouldThrowException_WhenAuctionIdIsInvalid()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _auctionService.CloseAuctionAsync(0, TestContext.Current.CancellationToken));
        }

        [Fact(Skip = "TODO: Check reason ")]
        public async Task GetActiveAuctionsAsync_ShouldReturnActiveAuctions()
        {
            // Arrange
            var activeAuctions = new List<Auction>
            {
                new Auction(Fixtures.GetHatchback()),
                new Auction(Fixtures.GetHatchback())
            };
            _auctionRepositoryMock.Setup(x => x.GetAllAuctionsAsync(true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(activeAuctions);

            // Act
            var result = await _auctionService.GetActiveAuctionsAsync(cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(2, result.Count());
            _auctionRepositoryMock.Verify(x => x.GetAllAuctionsAsync(true, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
