namespace AutoAuction.Application.DTOs
{
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
