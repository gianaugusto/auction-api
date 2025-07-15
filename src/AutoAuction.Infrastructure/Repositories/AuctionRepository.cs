using AutoAuction.Domain;
using AutoAuction.Domain.Repositories;
using AutoAuction.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoAuction.Infrastructure.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly List<Auction> _auctions = new();

        public void AddAuction(Auction auction)
        {
            if (auction == null)
                throw new ArgumentNullException(nameof(auction));

            _auctions.Add(auction);
        }

        public Auction GetAuctionById(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Auction ID must be greater than zero");

            var auction = _auctions.FirstOrDefault(a => a.Id == id);
            if (auction == null)
                throw new AuctionNotFoundException(id);

            return auction;
        }

        public IEnumerable<Auction> GetAllAuctions()
        {
            return _auctions.AsReadOnly();
        }
    }
}
