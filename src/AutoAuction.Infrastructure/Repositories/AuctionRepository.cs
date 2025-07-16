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
    public class AuctionRepository(DefaultContext context) : IAuctionRepository
    {
        private readonly DefaultContext _context = context;

        public async Task AddAuctionAsync(Auction auction, CancellationToken cancellationToken = default)
        {
            if (auction == null)
                throw new ArgumentNullException(nameof(auction));

            await _context.Auctions.AddAsync(auction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Auction> GetAuctionByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Auction ID must be greater than zero");

            var auction = await _context.Auctions.FindAsync(new object[] { id }, cancellationToken);

            if (auction == null)
                throw new AuctionNotFoundException(id);

            return auction;
        }

        public async Task<IEnumerable<Auction>> GetAllAuctionsAsync(bool isActive = true, CancellationToken cancellationToken = default)
        {
            return await _context.Auctions.Where(o => o.IsActive == isActive).ToListAsync(cancellationToken);
        }

        public async Task PlaceBidAsync(int auctionId, string bidderId, decimal bidAmount, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var auction = await _context.Auctions.FindAsync([auctionId], cancellationToken);

            if (auction == null)
                throw new AuctionNotFoundException(auctionId);

            auction.PlaceBid(bidderId, bidAmount);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        public async Task StartAuctionForVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var existingAuction = await _context.Auctions
                .Where(a => a.Vehicle.Id == vehicle.Id && a.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingAuction != null)
                throw new VehicleAlreadyInAuctionException(vehicle.Id);

            var auction = new Auction(vehicle);
            _context.Auctions.Add(auction);

            auction.StartAuction(); // updates IsActive

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        
        public async Task CloseAuctionAsync(int auctionId, CancellationToken cancellationToken = default)
        {
            var auction = await _context.Auctions
                .FirstOrDefaultAsync(a => a.Id == auctionId, cancellationToken);

            if (auction == null)
                throw new AuctionNotFoundException(auctionId);

            auction.CloseAuction();

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
