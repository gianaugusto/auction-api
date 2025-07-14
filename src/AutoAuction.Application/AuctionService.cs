using AutoAuction.Domain;

namespace AutoAuction.Application
{
    public class AuctionService
    {
        public Auction CreateAuction(string name, decimal startingBid)
        {
            return new Auction
            {
                Name = name,
                StartingBid = startingBid,
                CurrentBid = startingBid,
                IsActive = true
            };
        }
    }
}
