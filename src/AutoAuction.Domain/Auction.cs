using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoAuction.Domain
{
    public class Auction
    {
        public Vehicle Vehicle { get; private set; }
        public bool IsActive { get; private set; }
        public decimal CurrentHighestBid { get; private set; }
        private List<Bid> bids = new List<Bid>();

        public Auction(Vehicle vehicle)
        {
            Vehicle = vehicle ?? throw new ArgumentNullException(nameof(vehicle));
            CurrentHighestBid = vehicle.StartingBid;
        }

        public void StartAuction()
        {
            if (IsActive)
                throw new InvalidOperationException("Auction is already active");

            IsActive = true;
        }

        public void PlaceBid(string bidderId, decimal bidAmount)
        {
            if (!IsActive)
                throw new InvalidOperationException("Auction is not active");

            if (bidAmount <= CurrentHighestBid)
                throw new ArgumentException("Bid amount must be greater than the current highest bid", nameof(bidAmount));

            var bid = new Bid(bidderId, bidAmount);
            bids.Add(bid);
            CurrentHighestBid = bidAmount;
        }

        public void CloseAuction()
        {
            if (!IsActive)
                throw new InvalidOperationException("Auction is not active");

            IsActive = false;
        }

        public IEnumerable<Bid> GetBids()
        {
            return bids.AsReadOnly();
        }

        public class Bid
        {
            public string BidderId { get; private set; }
            public decimal BidAmount { get; private set; }
            public DateTime BidTime { get; private set; }

            public Bid(string bidderId, decimal bidAmount)
            {
                BidderId = bidderId ?? throw new ArgumentNullException(nameof(bidderId));
                BidAmount = bidAmount;
                BidTime = DateTime.UtcNow;
            }
        }
    }
}
