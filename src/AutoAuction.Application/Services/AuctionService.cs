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

            await _auctionRepository.StartAuctionForVehicleAsync(vehicle, cancellationToken);
        }

        public async Task PlaceBidAsync(int auctionId, string bidderId, decimal bidAmount, CancellationToken cancellationToken = default)
        {
            if (auctionId <= 0)
                throw new ArgumentOutOfRangeException(nameof(auctionId), "Auction ID must be greater than zero");

            if (string.IsNullOrWhiteSpace(bidderId))
                throw new ArgumentException("Bidder ID cannot be null or empty", nameof(bidderId));

            if (bidAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(bidAmount), "Bid amount must be greater than zero");

            await _auctionRepository.PlaceBidAsync(auctionId,bidderId,bidAmount, cancellationToken);
        }

        public async Task CloseAuctionAsync(int auctionId, CancellationToken cancellationToken = default)
        {
            if (auctionId <= 0)
                throw new ArgumentOutOfRangeException(nameof(auctionId));

            await _auctionRepository.CloseAuctionAsync(auctionId, cancellationToken);
        }

        public async Task<IEnumerable<Auction>> GetActiveAuctionsAsync(bool active = true, CancellationToken cancellationToken = default)
        {
            var allAuctions = await _auctionRepository.GetAllAuctionsAsync(active,cancellationToken);
            return allAuctions.Where(a => a.IsActive);
        }
    }
}
