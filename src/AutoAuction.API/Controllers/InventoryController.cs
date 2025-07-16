using Microsoft.AspNetCore.Mvc;
using AutoAuction.Application.DTOs;
using AutoAuction.Application;
using AutoAuction.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoAuction.Domain;

namespace AutoAuction.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
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

            await _inventoryService.AddVehicleAsync(vehicleDto, cancellationToken);
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

            var vehicles = await _inventoryService.SearchVehiclesAsync(type?.ToString(), manufacturer, model, year, cancellationToken);
            return Ok(new ApiResponseWithData<IEnumerable<Vehicle>> { Success = true, Message = "Vehicles retrieved successfully", Data = vehicles });
        }
    }
}
