using Microsoft.AspNetCore.Mvc;
using AutoAuction.Application.DTOs;
using AutoAuction.Application;
using AutoAuction.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

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
            if (vehicleDto == null)
            {
                return BadRequest(new { Error = "VehicleDto cannot be null" });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }

            _auctionService.AddVehicle(vehicleDto);
            return Ok();
        }

        [HttpGet("vehicles")]
        public IActionResult SearchVehicles([FromQuery] VehicleType? type = null, [FromQuery] string manufacturer = null, [FromQuery] string model = null, [FromQuery] int? year = null)
        {
            if (year != null && year <= 0)
            {
                return BadRequest(new { Error = "Year must be greater than zero" });
            }

            if (type != null && !Enum.IsDefined(typeof(VehicleType), type))
            {
                return BadRequest(new { Error = "Invalid vehicle type" });
            }

            var vehicles = _auctionService.SearchVehicles(type?.ToString(), manufacturer, model, year);
            return Ok(vehicles);
        }

        [HttpPost("auctions")]
        public IActionResult StartAuction([FromBody] StartAuctionDto startAuctionDto)
        {
            if (startAuctionDto == null)
            {
                return BadRequest(new { Error = "StartAuctionDto cannot be null" });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }

            _auctionService.StartAuction(startAuctionDto.VehicleId);
            return Ok();
        }

        [HttpPost("auctions/{auctionId}/bids")]
        public IActionResult PlaceBid([FromRoute] int auctionId, [FromBody] PlaceBidDto placeBidDto)
        {
            if (auctionId <= 0)
            {
                return BadRequest(new { Error = "Auction ID must be greater than zero" });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }

            _auctionService.PlaceBid(auctionId, placeBidDto.BidderId, placeBidDto.BidAmount);
            return Ok();
        }

        [HttpPost("auctions/{auctionId}/close")]
        public IActionResult CloseAuction(int auctionId)
        {
            if (auctionId <= 0)
            {
                return BadRequest(new { Error = "Auction ID must be greater than zero" });
            }

            _auctionService.CloseAuction(auctionId);
            return Ok();
        }

        [HttpGet("auctions/active")]
        public IActionResult GetActiveAuctions()
        {
            var auctions = _auctionService.GetActiveAuctions();
            if (auctions == null || !auctions.Any())
            {
                return Ok(new { Message = "No active auctions found", Auctions = new List<Auction>() });
            }

            return Ok(new { Message = "Active auctions retrieved successfully", Auctions = auctions });
        }
    }
}
