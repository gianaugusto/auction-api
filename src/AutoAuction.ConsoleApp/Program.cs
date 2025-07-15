using System;
using AutoAuction.Application;
using AutoAuction.Domain;

namespace AutoAuction.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var auctionService = new AuctionService();

            // Add vehicles
            var hatchback = new Hatchback("V001", "Toyota", "Yaris", 2020, 5000m, 5);
            var suv = new SUV("V002", "Ford", "Explorer", 2019, 10000m, 7);
            var sedan = new Sedan("V003", "BMW", "3 Series", 2021, 15000m, 4);
            var truck = new Truck("V004", "Ford", "F-150", 2018, 20000m, 1.5m);

            auctionService.AddVehicle(hatchback);
            auctionService.AddVehicle(suv);
            auctionService.AddVehicle(sedan);
            auctionService.AddVehicle(truck);

            // Search vehicles
            Console.WriteLine("Searching for SUVs:");
            foreach (var vehicle in auctionService.SearchVehicles(VehicleType.SUV.ToString()))
            {
                vehicle.DisplayInfo();
            }

            // Start auction
            auctionService.StartAuction("V002");

            // Place bids
            auctionService.PlaceBid("V002", "Bidder1", 11000m);
            auctionService.PlaceBid("V002", "Bidder2", 12000m);

            // Close auction
            auctionService.CloseAuction("V002");

            Console.WriteLine("Auction closed. Final bid: 12000");
        }
    }
}
