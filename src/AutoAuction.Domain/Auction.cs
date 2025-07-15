using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoAuction.Domain.Exceptions;

namespace AutoAuction.Domain
{
    public class Auction
    {
        public int Id { get; private set; }

        public Vehicle Vehicle { get; private set; }

        public bool IsActive { get; private set; }

        public decimal CurrentHighestBid { get; private set; }

        private List<Bid> bids = new List<Bid>();

        private static int nextId = 1;

        [Timestamp] // just to check if updated
        public byte[] RowVersion { get; set; }

        public Auction(Vehicle vehicle)
        {
            Id = nextId++;
            Vehicle = vehicle ?? throw new ArgumentNullException(nameof(vehicle));
            CurrentHighestBid = vehicle.StartingBid;
        }

        public void StartAuction()
        {
            if (IsActive)
                throw new AuctionAlreadyActiveException();

            IsActive = true;
        }

        public void PlaceBid(string bidderId, decimal bidAmount)
        {
            if (!IsActive)
                throw new AuctionNotActiveException();

            if (string.IsNullOrWhiteSpace(bidderId))
                throw new ArgumentException("Bidder ID cannot be null or empty", nameof(bidderId));

            if (bidAmount <= 0)
                throw new ArgumentException("Bid amount must be greater than zero", nameof(bidAmount));

            if (bidAmount <= CurrentHighestBid)
                throw new InvalidBidAmountException(bidAmount, CurrentHighestBid);

            var bid = new Bid(bidderId, bidAmount);
            bids.Add(bid);
            CurrentHighestBid = bidAmount;
        }

        public void CloseAuction()
        {
            if (!IsActive)
                throw new AuctionNotActiveException();

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
