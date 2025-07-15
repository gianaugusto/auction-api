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
    public class AuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly Dictionary<string, Vehicle> inventory = new Dictionary<string, Vehicle>();

        public AuctionService(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
        }

        public void AddVehicle(VehicleDto vehicleDto)
        {
            if (vehicleDto == null)
                throw new ArgumentNullException(nameof(vehicleDto));

            Vehicle vehicle = vehicleDto.ToDomain();

            if (inventory.ContainsKey(vehicle.Id))
                throw new ArgumentException("Vehicle with this ID already exists", nameof(vehicle));

            inventory[vehicle.Id] = vehicle;
        }

        public Task AddVehicleAsync(VehicleDto vehicleDto, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => AddVehicle(vehicleDto), cancellationToken);
        }

        public IEnumerable<Vehicle> SearchVehicles(string type = null, string manufacturer = null, string model = null, int? year = null)
        {
            if (year != null && year <= 0)
                throw new ArgumentOutOfRangeException(nameof(year), "Year must be greater than zero");

            return inventory.Values.Where(v =>
                (type == null || v.GetType().Name.Equals(type, StringComparison.OrdinalIgnoreCase)) &&
                (manufacturer == null || v.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase)) &&
                (model == null || v.Model.Equals(model, StringComparison.OrdinalIgnoreCase)) &&
                (year == null || v.Year == year)
            );
        }

        public Task<IEnumerable<Vehicle>> SearchVehiclesAsync(string type = null, string manufacturer = null, string model = null, int? year = null, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => SearchVehicles(type, manufacturer, model, year), cancellationToken);
        }

        public void StartAuction(string vehicleId)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be null or empty", nameof(vehicleId));

            if (!inventory.ContainsKey(vehicleId))
                throw new VehicleNotFoundException(vehicleId);

            var vehicle = inventory[vehicleId];

            // Check if the vehicle is already in an active auction
            var existingAuction = _auctionRepository.GetAllAuctions()
                .FirstOrDefault(a => a.Vehicle.Id == vehicleId && a.IsActive);

            if (existingAuction != null)
                throw new VehicleAlreadyInAuctionException(vehicleId);

            var auction = new Auction(vehicle);
            _auctionRepository.AddAuction(auction);
            auction.StartAuction();
        }

        public Task StartAuctionAsync(string vehicleId, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => StartAuction(vehicleId), cancellationToken);
        }

        public void PlaceBid(int auctionId, string bidderId, decimal bidAmount)
        {
            if (auctionId <= 0)
                throw new ArgumentOutOfRangeException(nameof(auctionId), "Auction ID must be greater than zero");

            if (string.IsNullOrWhiteSpace(bidderId))
                throw new ArgumentException("Bidder ID cannot be null or empty", nameof(bidderId));

            if (bidAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(bidAmount), "Bid amount must be greater than zero");

            var auction = _auctionRepository.GetAuctionById(auctionId);
            if (auction == null)
                throw new AuctionNotFoundException(auctionId);

            auction.PlaceBid(bidderId, bidAmount);
        }

        public Task PlaceBidAsync(int auctionId, string bidderId, decimal bidAmount, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => PlaceBid(auctionId, bidderId, bidAmount), cancellationToken);
        }

        public void CloseAuction(int auctionId)
        {
            if (auctionId <= 0)
                throw new ArgumentOutOfRangeException(nameof(auctionId), "Auction ID must be greater than zero");

            var auction = _auctionRepository.GetAuctionById(auctionId);
            if (auction == null)
                throw new AuctionNotFoundException(auctionId);

            auction.CloseAuction();
        }

        public Task CloseAuctionAsync(int auctionId, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => CloseAuction(auctionId), cancellationToken);
        }

        public IEnumerable<Auction> GetActiveAuctions()
        {
            return _auctionRepository.GetAllAuctions().Where(a => a.IsActive);
        }

        public Task<IEnumerable<Auction>> GetActiveAuctionsAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(() => GetActiveAuctions(), cancellationToken);
        }
    }
}
