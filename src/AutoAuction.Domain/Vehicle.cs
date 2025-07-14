using System;

namespace AutoAuction.Domain
{
    public abstract class Vehicle
    {
        public string Id { get; private set; }
        public string Manufacturer { get; private set; }
        public string Model { get; private set; }
        public int Year { get; private set; }
        public decimal StartingBid { get; private set; }

        protected Vehicle(string id, string manufacturer, string model, int year, decimal startingBid)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Vehicle ID cannot be null or empty", nameof(id));

            Id = id;
            Manufacturer = manufacturer ?? throw new ArgumentNullException(nameof(manufacturer));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Year = year;
            StartingBid = startingBid;
        }

        public abstract void DisplayInfo();
    }
}
