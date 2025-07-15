using Microsoft.AspNetCore.Mvc;
using AutoAuction.Application.DTOs;
using AutoAuction.Application;
using AutoAuction.Domain;
using AutoAuction.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task<IActionResult> AddVehicle([FromBody] VehicleDto vehicleDto, CancellationToken cancellationToken = default)
        {
            if (vehicleDto == null)
            {
                return BadRequest(new ApiResponse { Success = false, Message = "VehicleDto cannot be null" });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new ApiResponseWithData<IEnumerable<string>> { Success = false, Message = "Validation errors", Data = errors });
            }

            await _auctionService.AddVehicleAsync(vehicleDto, cancellationToken);
            return Ok(new ApiResponse { Success = true, Message = "Vehicle added successfully" });
        }

        [HttpGet("vehicles")]
        public async Task<IActionResult> SearchVehicles([FromQuery] VehicleType? type = null, [FromQuery] string manufacturer = null, [FromQuery] string model = null, [FromQuery] int? year = null, CancellationToken cancellationToken = default)
        {
            if (year != null && year <= 0)
            {
                return BadRequest(new ApiResponse { Success = false, Message = "Year must be greater than zero" });
            }

            if (type != null && !Enum.IsDefined(typeof(VehicleType), type))
            {
                return BadRequest(new ApiResponse { Success = false, Message = "Invalid vehicle type" });
            }

            var vehicles = await _auctionService.SearchVehiclesAsync(type?.ToString(), manufacturer, model, year, cancellationToken);
            return Ok(new ApiResponseWithData<IEnumerable<Vehicle>> { Success = true, Message = "Vehicles retrieved successfully", Data = vehicles });
        }

        [HttpPost("auctions")]
        public async Task<IActionResult> StartAuction([FromBody] StartAuctionDto startAuctionDto, CancellationToken cancellationToken = default)
        {
            if (startAuctionDto == null)
            {
                return BadRequest(new ApiResponse { Success = false, Message = "StartAuctionDto cannot be null" });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new ApiResponseWithData<IEnumerable<string>> { Success = false, Message = "Validation errors", Data = errors });
            }

            await _auctionService.StartAuctionAsync(startAuctionDto.VehicleId, cancellationToken);
            return Ok(new ApiResponse { Success = true, Message = "Auction started successfully" });
        }

        [HttpPost("auctions/{auctionId}/bids")]
        public async Task<IActionResult> PlaceBid([FromRoute] int auctionId, [FromBody] PlaceBidDto placeBidDto, CancellationToken cancellationToken = default)
        {
            if (auctionId <= 0)
            {
                return BadRequest(new ApiResponse { Success = false, Message = "Auction ID must be greater than zero" });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new ApiResponseWithData<IEnumerable<string>> { Success = false, Message = "Validation errors", Data = errors });
            }

            await _auctionService.PlaceBidAsync(auctionId, placeBidDto.BidderId, placeBidDto.BidAmount, cancellationToken);
            return Ok(new ApiResponse { Success = true, Message = "Bid placed successfully" });
        }

        [HttpPost("auctions/{auctionId}/close")]
        public async Task<IActionResult> CloseAuction(int auctionId, CancellationToken cancellationToken = default)
        {
            if (auctionId <= 0)
            {
                return BadRequest(new ApiResponse { Success = false, Message = "Auction ID must be greater than zero" });
            }

            await _auctionService.CloseAuctionAsync(auctionId, cancellationToken);
            return Ok(new ApiResponse { Success = true, Message = "Auction closed successfully" });
        }

        [HttpGet("auctions/active")]
        public async Task<IActionResult> GetActiveAuctions(CancellationToken cancellationToken = default)
        {
            var auctions = await _auctionService.GetActiveAuctionsAsync(cancellationToken);
            if (auctions == null || !auctions.Any())
            {
                return Ok(new ApiResponseWithData<List<Auction>> { Success = true, Message = "No active auctions found", Data = new List<Auction>() });
            }

            return Ok(new ApiResponseWithData<IEnumerable<Auction>> { Success = true, Message = "Active auctions retrieved successfully", Data = auctions });
        }
    }
}
