using Microsoft.AspNetCore.Mvc;
using AutoAuction.Application.DTOs;
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
            _auctionService.AddVehicle(vehicleDto);
            return Ok();
        }

        [HttpGet("vehicles")]
        public IActionResult SearchVehicles([FromQuery] VehicleType? type = null, [FromQuery] string manufacturer = null, [FromQuery] string model = null, [FromQuery] int? year = null)
        {
            var vehicles = _auctionService.SearchVehicles(type?.ToString(), manufacturer, model, year);
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
    }
}
