using System;

namespace AutoAuction.Domain
{
    public class Hatchback : Vehicle
    {
        public int NumberOfDoors { get; private set; }

        public Hatchback(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
            : base(id, manufacturer, model, year, startingBid)
        {
            NumberOfDoors = numberOfDoors;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Hatchback - ID: {Id}, Manufacturer: {Manufacturer}, Model: {Model}, Year: {Year}, Starting Bid: {StartingBid:C}, Doors: {NumberOfDoors}");
        }
    }

    public class Sedan : Vehicle
    {
        public int NumberOfDoors { get; private set; }

        public Sedan(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
            : base(id, manufacturer, model, year, startingBid)
        {
            NumberOfDoors = numberOfDoors;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Sedan - ID: {Id}, Manufacturer: {Manufacturer}, Model: {Model}, Year: {Year}, Starting Bid: {StartingBid:C}, Doors: {NumberOfDoors}");
        }
    }

    public class SUV : Vehicle
    {
        public int NumberOfSeats { get; private set; }

        public SUV(string id, string manufacturer, string model, int year, decimal startingBid, int numberOfSeats)
            : base(id, manufacturer, model, year, startingBid)
        {
            NumberOfSeats = numberOfSeats;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"SUV - ID: {Id}, Manufacturer: {Manufacturer}, Model: {Model}, Year: {Year}, Starting Bid: {StartingBid:C}, Seats: {NumberOfSeats}");
        }
    }

    public class Truck : Vehicle
    {
        public decimal LoadCapacity { get; private set; }

        public Truck(string id, string manufacturer, string model, int year, decimal startingBid, decimal loadCapacity)
            : base(id, manufacturer, model, year, startingBid)
        {
            LoadCapacity = loadCapacity;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Truck - ID: {Id}, Manufacturer: {Manufacturer}, Model: {Model}, Year: {Year}, Starting Bid: {StartingBid:C}, Load Capacity: {LoadCapacity} tons");
        }
    }
}
