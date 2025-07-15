using System;

namespace AutoAuction.Domain.Exceptions
{
    public class AuctionException : Exception
    {
        public AuctionException(string message) : base(message)
        {
        }

        public AuctionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class VehicleNotFoundException : AuctionException
    {
        public VehicleNotFoundException(string vehicleId) : base($"Vehicle with ID '{vehicleId}' not found in inventory")
        {
        }
    }

    public class VehicleAlreadyInAuctionException : AuctionException
    {
        public VehicleAlreadyInAuctionException(string vehicleId) : base($"Vehicle with ID '{vehicleId}' is already in an active auction")
        {
        }
    }

    public class InvalidBidAmountException : AuctionException
    {
        public InvalidBidAmountException(decimal bidAmount, decimal currentHighestBid)
            : base($"Bid amount {bidAmount} must be greater than the current highest bid {currentHighestBid}")
        {
        }
    }

    public class AuctionNotFoundException : AuctionException
    {
        public AuctionNotFoundException(int auctionId) : base($"Auction with ID {auctionId} not found")
        {
        }
    }

    public class AuctionAlreadyActiveException : AuctionException
    {
        public AuctionAlreadyActiveException() : base("Auction is already active")
        {
        }
    }

    public class AuctionNotActiveException : AuctionException
    {
        public AuctionNotActiveException() : base("Auction is not active")
        {
        }
    }
}
