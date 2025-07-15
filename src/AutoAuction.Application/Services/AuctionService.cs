using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoAuction.Domain;
using AutoAuction.Domain.Repositories;
using AutoAuction.Application.DTOs;
using AutoAuction.Application.Mappers;
using AutoAuction.Domain.Exceptions;
using System.Transactions;

namespace AutoAuction.Application
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IInventoryService _inventoryService;

        public AuctionService(IAuctionRepository auctionRepository, IInventoryService inventoryService)
        {
            _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
        }

        public async Task StartAuctionAsync(string vehicleId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be null or empty", nameof(vehicleId));

            var vehicle = await _inventoryService.GetVehicleByIdAsync(vehicleId, cancellationToken);

            if (vehicle == null)
                throw new VehicleNotFoundException(vehicleId);

            // Check if the vehicle is already in an active auction
            var allAuctions = await _auctionRepository.GetAllAuctionsAsync(true, cancellationToken);
            var existingAuction = allAuctions.FirstOrDefault(a => a.Vehicle.Id == vehicleId && a.IsActive);

            if (existingAuction != null)
                throw new VehicleAlreadyInAuctionException(vehicleId);

            var auction = new Auction(vehicle);
            await _auctionRepository.AddAuctionAsync(auction, cancellationToken);

            auction.StartAuction();
        }

        public async Task PlaceBidAsync(int auctionId, string bidderId, decimal bidAmount, CancellationToken cancellationToken = default)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (auctionId <= 0)
                    throw new ArgumentOutOfRangeException(nameof(auctionId), "Auction ID must be greater than zero");

                if (string.IsNullOrWhiteSpace(bidderId))
                    throw new ArgumentException("Bidder ID cannot be null or empty", nameof(bidderId));

                if (bidAmount <= 0)
                    throw new ArgumentOutOfRangeException(nameof(bidAmount), "Bid amount must be greater than zero");

                var auction = await _auctionRepository.GetAuctionByIdAsync(auctionId, cancellationToken);

                if (auction == null)
                    throw new AuctionNotFoundException(auctionId);

                auction.PlaceBid(bidderId, bidAmount);
                
                scope.Complete();
            }
            
        }

        public async Task CloseAuctionAsync(int auctionId, CancellationToken cancellationToken = default)
        {
            if (auctionId <= 0)
                throw new ArgumentOutOfRangeException(nameof(auctionId), "Auction ID must be greater than zero");

            var auction = await _auctionRepository.GetAuctionByIdAsync(auctionId, cancellationToken);
            if (auction == null)
                throw new AuctionNotFoundException(auctionId);

            auction.CloseAuction();
        }

        public async Task<IEnumerable<Auction>> GetActiveAuctionsAsync(bool active = true, CancellationToken cancellationToken = default)
        {
            var allAuctions = await _auctionRepository.GetAllAuctionsAsync(active,cancellationToken);
            return allAuctions.Where(a => a.IsActive);
        }
    }
}
