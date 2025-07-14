using Microsoft.AspNetCore.Mvc;
using AutoAuction.Application;
using AutoAuction.Domain;
using System.Collections.Generic;

namespace AutoAuction.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly AuctionService _auctionService;

        public AuctionController(AuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpPost("vehicles")]
        public IActionResult AddVehicle([FromBody] VehicleDto vehicleDto)
        {
            Vehicle vehicle = vehicleDto.Type switch
            {
                "Hatchback" => new Hatchback(vehicleDto.Id, vehicleDto.Manufacturer, vehicleDto.Model, vehicleDto.Year, vehicleDto.StartingBid, vehicleDto.NumberOfDoors ?? 0),
                "Sedan" => new Sedan(vehicleDto.Id, vehicleDto.Manufacturer, vehicleDto.Model, vehicleDto.Year, vehicleDto.StartingBid, vehicleDto.NumberOfDoors ?? 0),
                "SUV" => new SUV(vehicleDto.Id, vehicleDto.Manufacturer, vehicleDto.Model, vehicleDto.Year, vehicleDto.StartingBid, vehicleDto.NumberOfSeats ?? 0),
                "Truck" => new Truck(vehicleDto.Id, vehicleDto.Manufacturer, vehicleDto.Model, vehicleDto.Year, vehicleDto.StartingBid, vehicleDto.LoadCapacity ?? 0m),
                _ => throw new System.ArgumentException("Invalid vehicle type")
            };

            _auctionService.AddVehicle(vehicle);
            return Ok();
        }

        [HttpGet("vehicles")]
        public IActionResult SearchVehicles([FromQuery] string type = null, [FromQuery] string manufacturer = null, [FromQuery] string model = null, [FromQuery] int? year = null)
        {
            var vehicles = _auctionService.SearchVehicles(type, manufacturer, model, year);
            return Ok(vehicles);
        }

        [HttpPost("auctions")]
        public IActionResult StartAuction([FromBody] StartAuctionDto startAuctionDto)
        {
            _auctionService.StartAuction(startAuctionDto.VehicleId);
            return Ok();
        }

        [HttpPost("auctions/{auctionId}/bids")]
        public IActionResult PlaceBid(int auctionId, [FromBody] PlaceBidDto placeBidDto)
        {
            _auctionService.PlaceBid(auctionId, placeBidDto.BidderId, placeBidDto.BidAmount);
            return Ok();
        }

        [HttpPost("auctions/{auctionId}/close")]
        public IActionResult CloseAuction(int auctionId)
        {
            _auctionService.CloseAuction(auctionId);
            return Ok();
        }

        [HttpGet("auctions/active")]
        public IActionResult GetActiveAuctions()
        {
            var auctions = _auctionService.GetActiveAuctions();
            return Ok(auctions);
        }

        public class VehicleDto
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public string Manufacturer { get; set; }
            public string Model { get; set; }
            public int Year { get; set; }
            public decimal StartingBid { get; set; }
            public int? NumberOfDoors { get; set; }
            public int? NumberOfSeats { get; set; }
            public decimal? LoadCapacity { get; set; }
        }

        public class StartAuctionDto
        {
            public string VehicleId { get; set; }
        }

        public class PlaceBidDto
        {
            public string BidderId { get; set; }
            public decimal BidAmount { get; set; }
        }
    }
}
