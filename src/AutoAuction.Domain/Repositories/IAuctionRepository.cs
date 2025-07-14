using System.Collections.Generic;

namespace AutoAuction.Domain.Repositories
{
    public interface IAuctionRepository
    {
        void AddAuction(Auction auction);
        Auction GetAuctionById(int id);
        IEnumerable<Auction> GetAllAuctions();
    }
}
