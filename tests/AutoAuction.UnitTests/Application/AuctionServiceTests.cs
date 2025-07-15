//using System;
//using System.Linq;
//using AutoAuction.Application;
//using AutoAuction.Application.DTOs;
//using AutoAuction.Domain;
//using AutoAuction.Domain.Exceptions;
//using AutoAuction.Domain.Repositories;
//using Moq;
//using Xunit;

//namespace AutoAuction.UnitTests.Application
//{
//    public class AuctionServiceTests
//    {
//        [Fact]
//        public void AddVehicle_ShouldAddVehicleToInventory()
//        {
//            // Arrange
//            var mockRepository = new Mock<IAuctionRepository>();
//            var mockInventoryRepository = new Mock<IInventoryService>();
//            var service = new AuctionService(mockRepository.Object, mockInventoryRepository.Object);
//            var vehicleDto = new VehicleDto
//            {
//                Id = "V001",
//                Type = VehicleType.Hatchback,
//                Manufacturer = "Toyota",
//                Model = "Yaris",
//                Year = 2020,
//                StartingBid = 5000m,
//                NumberOfDoors = 5
//            };

//            // Act
//            service.AddVehicle(vehicleDto);

//            // Assert
//            var vehicles = service.SearchVehicles();
//            Assert.Single(vehicles);
//            Assert.Equal("V001", vehicles.First().Id);
//        }

//        [Fact]
//        public void AddVehicle_ShouldThrowException_WhenVehicleIdAlreadyExists()
//        {
//            // Arrange
//            var mockRepository = new Mock<IAuctionRepository>();
//            var service = new AuctionService(mockRepository.Object);
//            var vehicleDto1 = new VehicleDto
//            {
//                Id = "V001",
//                Type = VehicleType.Hatchback,
//                Manufacturer = "Toyota",
//                Model = "Yaris",
//                Year = 2020,
//                StartingBid = 5000m,
//                NumberOfDoors = 5
//            };
//            var vehicleDto2 = new VehicleDto
//            {
//                Id = "V001",
//                Type = VehicleType.Hatchback,
//                Manufacturer = "Honda",
//                Model = "Civic",
//                Year = 2019,
//                StartingBid = 6000m,
//                NumberOfDoors = 5
//            };

//            // Act & Assert
//            service.AddVehicle(vehicleDto1);
//            Assert.Throws<ArgumentException>(() => service.AddVehicle(vehicleDto2));
//        }

//        [Fact]
//        public void StartAuction_ShouldStartAuctionForVehicle()
//        {
//            // Arrange
//            var mockRepository = new Mock<IAuctionRepository>();
//            var service = new AuctionService(mockRepository.Object);
//            var vehicleDto = new VehicleDto
//            {
//                Id = "V001",
//                Type = VehicleType.Hatchback,
//                Manufacturer = "Toyota",
//                Model = "Yaris",
//                Year = 2020,
//                StartingBid = 5000m,
//                NumberOfDoors = 5
//            };
//            service.AddVehicle(vehicleDto);

//            // Act
//            service.StartAuction("V001");

//            // Assert
//            mockRepository.Verify(r => r.AddAuction(It.IsAny<Auction>()), Times.Once);
//        }

//        [Fact]
//        public void PlaceBid_ShouldPlaceBidOnActiveAuction()
//        {
//            // Arrange
//            var mockRepository = new Mock<IAuctionRepository>();
//            var auction = new Auction(new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5));
//            auction.StartAuction();
//            mockRepository.Setup(r => r.GetAuctionById(1)).Returns(auction);

//            var service = new AuctionService(mockRepository.Object);

//            // Act
//            service.PlaceBid(1, "Bidder1", 6000m);

//            // Assert
//            Assert.Equal(6000m, auction.CurrentHighestBid);
//        }

//        [Fact]
//        public void PlaceBid_ShouldThrowException_WhenAuctionIsNotActive()
//        {
//            // Arrange
//            var mockRepository = new Mock<IAuctionRepository>();
//            var auction = new Auction(new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5));
//            mockRepository.Setup(r => r.GetAuctionById(1)).Returns(auction);

//            var service = new AuctionService(mockRepository.Object);

//            // Act & Assert
//            Assert.Throws<AuctionNotActiveException>(() => service.PlaceBid(1, "Bidder1", 6000m));
//        }

//        [Fact]
//        public void PlaceBid_ShouldThrowException_WhenBidAmountIsInvalid()
//        {
//            // Arrange
//            var mockRepository = new Mock<IAuctionRepository>();
//            var auction = new Auction(new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5));
//            auction.StartAuction();
//            mockRepository.Setup(r => r.GetAuctionById(1)).Returns(auction);

//            var service = new AuctionService(mockRepository.Object);

//            // Act & Assert
//            Assert.Throws<InvalidBidAmountException>(() => service.PlaceBid(1, "Bidder1", 4000m));
//        }

//        [Fact]
//        public void Handle_ShouldStartAuctionForVehicle()
//        {
//            // Arrange
//            var mockRepository = new Mock<IAuctionRepository>();
//            var service = new AuctionService(mockRepository.Object);
//            var vehicleDto = new VehicleDto
//            {
//                Id = "V001",
//                Type = VehicleType.Hatchback,
//                Manufacturer = "Toyota",
//                Model = "Yaris",
//                Year = 2020,
//                StartingBid = 5000m,
//                NumberOfDoors = 5
//            };
//            service.AddVehicle(vehicleDto);

//            // Act
//            service.StartAuction("V001");

//            // Assert
//            mockRepository.Verify(r => r.AddAuction(It.IsAny<Auction>()), Times.Once);
//        }
//    }
//}
