using AutoAuction.Domain;
using AutoAuction.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace AutoAuction.Infrastructure.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly List<Auction> _auctions = new();

        public void AddAuction(Auction auction)
        {
            _auctions.Add(auction);
        }

        public Auction GetAuctionById(int id)
        {
            return _auctions.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Auction> GetAllAuctions()
        {
            return _auctions;
        }
    }
}
