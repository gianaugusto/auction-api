using System;
using System.ComponentModel.DataAnnotations;
using AutoAuction.Application.Validation;

namespace AutoAuction.Application.DTOs
{
    public class StartAuctionDto
    {
        [Required]
        [VehicleIdValidation]
        public string VehicleId { get; set; }
    }

    public class PlaceBidDto
    {
        [Required]
        [BidderIdValidation]
        public string BidderId { get; set; }

        [Required]
        [BidAmountValidation]
        public decimal BidAmount { get; set; }
    }
}
