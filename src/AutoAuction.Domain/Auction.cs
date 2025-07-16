using System;
using System.Collections.Generic;
using AutoAuction.Domain.Exceptions;

namespace AutoAuction.Domain
{
    public class Auction
    {
        private readonly List<Bid> _bids = new();
        private static int _nextId = 1;

        public int Id { get; private set; }
        public Vehicle Vehicle { get; private set; }
        public bool IsActive { get; private set; }
        public decimal CurrentHighestBid { get; private set; }
        public byte[] RowVersion { get; private set; }

        public IReadOnlyCollection<Bid> Bids => _bids.AsReadOnly();

        // EF Core constructor
        private Auction() { }

        public Auction(Vehicle vehicle)
        {
            Id = _nextId++;
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

            if (bidAmount <= CurrentHighestBid)
                throw new InvalidBidAmountException(bidAmount, CurrentHighestBid);

            var bid = new Bid(bidderId, bidAmount);
            _bids.Add(bid);
            CurrentHighestBid = bidAmount;
        }

        public void CloseAuction()
        {
            if (!IsActive)
                throw new AuctionNotActiveException();

            IsActive = false;
        }

        public class Bid
        {
            public string BidderId { get; private set; }
            public decimal BidAmount { get; private set; }
            public DateTime BidTime { get; private set; }

            private Bid() { } // EF Core

            public Bid(string bidderId, decimal bidAmount)
            {
                BidderId = bidderId ?? throw new ArgumentNullException(nameof(bidderId));
                BidAmount = bidAmount;
                BidTime = DateTime.UtcNow;
            }
        }
    }
}
