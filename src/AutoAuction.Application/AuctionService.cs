using System;
using System.Collections.Generic;
using System.Linq;
using AutoAuction.Domain;
using AutoAuction.Domain.Repositories;

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

        public void AddVehicle(Vehicle vehicle)
        {
            if (inventory.ContainsKey(vehicle.Id))
                throw new ArgumentException("Vehicle with this ID already exists", nameof(vehicle));

            inventory[vehicle.Id] = vehicle;
        }

        public IEnumerable<Vehicle> SearchVehicles(string type = null, string manufacturer = null, string model = null, int? year = null)
        {
            return inventory.Values.Where(v =>
                (type == null || v.GetType().Name.Equals(type, StringComparison.OrdinalIgnoreCase)) &&
                (manufacturer == null || v.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase)) &&
                (model == null || v.Model.Equals(model, StringComparison.OrdinalIgnoreCase)) &&
                (year == null || v.Year == year)
            );
        }

        public void StartAuction(string vehicleId)
        {
            if (!inventory.ContainsKey(vehicleId))
                throw new KeyNotFoundException("Vehicle not found in inventory");

            var vehicle = inventory[vehicleId];
            var auction = new Auction(vehicle);
            _auctionRepository.AddAuction(auction);
            auction.StartAuction();
        }

        public void PlaceBid(int auctionId, string bidderId, decimal bidAmount)
        {
            var auction = _auctionRepository.GetAuctionById(auctionId);
            if (auction == null)
                throw new KeyNotFoundException("Auction not found");

            auction.PlaceBid(bidderId, bidAmount);
        }

        public void CloseAuction(int auctionId)
        {
            var auction = _auctionRepository.GetAuctionById(auctionId);
            if (auction == null)
                throw new KeyNotFoundException("Auction not found");

            auction.CloseAuction();
        }

        public IEnumerable<Auction> GetActiveAuctions()
        {
            return _auctionRepository.GetAllAuctions().Where(a => a.IsActive);
        }
    }
}
