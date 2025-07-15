using AutoAuction.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using AutoAuction.Application.Validation;

namespace AutoAuction.Application.DTOs
{
    public class VehicleDto
    {
        [Required]
        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        [VehicleTypeValidation]
        public VehicleType Type { get; set; }

        [Required]
        [StringLength(100)]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(100)]
        public string Model { get; set; }

        [Required]
        [Range(1886, int.MaxValue)] // First car was made in 1886
        public int Year { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal StartingBid { get; set; }

        [Range(0, int.MaxValue)]
        public int? NumberOfDoors { get; set; }

        [Range(0, int.MaxValue)]
        public int? NumberOfSeats { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal? LoadCapacity { get; set; }
    }
}
