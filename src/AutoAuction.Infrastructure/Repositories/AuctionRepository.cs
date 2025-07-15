using AutoAuction.Domain;
using AutoAuction.Domain.Repositories;
using AutoAuction.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace AutoAuction.Infrastructure.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly ConcurrentBag<Auction> _auctions = new();

        public async Task AddAuctionAsync(Auction auction, CancellationToken cancellationToken = default)
        {
            if (auction == null)
                throw new ArgumentNullException(nameof(auction));

            // Simulate async operation
            await Task.Run(() => _auctions.Add(auction), cancellationToken);
        }

        public async Task<Auction> GetAuctionByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Auction ID must be greater than zero");

            // Simulate async operation
            var auction = await Task.Run(() => _auctions.FirstOrDefault(a => a.Id == id), cancellationToken);
            if (auction == null)
                throw new AuctionNotFoundException(id);

            return auction;
        }

        public async Task<IEnumerable<Auction>> GetAllAuctionsAsync(bool active = true, CancellationToken cancellationToken = default)
        {
            // Simulate async operation
            return await Task.Run(() => _auctions.Where(o => o.IsActive == active), cancellationToken);
        }
    }
}
