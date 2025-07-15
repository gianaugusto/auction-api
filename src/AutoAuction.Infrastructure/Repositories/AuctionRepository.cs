using AutoAuction.Domain;
using AutoAuction.Domain.Repositories;
using AutoAuction.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoAuction.Infrastructure.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly DbContextOptions<DefaultContext> _options;

        public AuctionRepository(DbContextOptions<DefaultContext> options)
        {
            _options = options;
        }

        public async Task AddAuctionAsync(Auction auction, CancellationToken cancellationToken = default)
        {
            if (auction == null)
                throw new ArgumentNullException(nameof(auction));

            using var context = new DefaultContext(_options);
            await context.Auctions.AddAsync(auction, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Auction> GetAuctionByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Auction ID must be greater than zero");

            using var context = new DefaultContext(_options);
            var auction = await context.Auctions.FindAsync(new object[] { id }, cancellationToken);

            if (auction == null)
                throw new AuctionNotFoundException(id);

            return auction;
        }

        public async Task<IEnumerable<Auction>> GetAllAuctionsAsync(bool isActive = true,CancellationToken cancellationToken = default)
        {
            using var context = new DefaultContext(_options);
            return await context.Auctions.Where(o => o.IsActive == isActive).ToListAsync(cancellationToken);
        }
    }
}
