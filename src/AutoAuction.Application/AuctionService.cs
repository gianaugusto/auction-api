using System;
using System.Collections.Generic;
using System.Linq;
using AutoAuction.Domain;

namespace AutoAuction.Application
{
    public class AuctionService
    {
        private readonly Dictionary<string, Vehicle> inventory = new Dictionary<string, Vehicle>();
        private readonly Dictionary<string, Auction> activeAuctions = new Dictionary<string, Auction>();

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

            if (activeAuctions.ContainsKey(vehicleId))
                throw new InvalidOperationException("Auction for this vehicle is already active");

            var vehicle = inventory[vehicleId];
            var auction = new Auction(vehicle);
            auction.StartAuction();
            activeAuctions[vehicleId] = auction;
        }

        public void PlaceBid(string vehicleId, string bidderId, decimal bidAmount)
        {
            if (!activeAuctions.ContainsKey(vehicleId))
                throw new InvalidOperationException("No active auction for this vehicle");

            var auction = activeAuctions[vehicleId];
            auction.PlaceBid(bidderId, bidAmount);
        }

        public void CloseAuction(string vehicleId)
        {
            if (!activeAuctions.ContainsKey(vehicleId))
                throw new InvalidOperationException("No active auction for this vehicle");

            var auction = activeAuctions[vehicleId];
            auction.CloseAuction();
            activeAuctions.Remove(vehicleId);
        }

        public Dictionary<string, Auction> GetActiveAuctions()
        {
            return activeAuctions;
        }
    }
}
