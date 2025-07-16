using System;
using AutoAuction.Domain.Exceptions;

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

            if (id.Length > 50)
                throw new ArgumentException("Vehicle ID cannot exceed 50 characters", nameof(id));

            if (string.IsNullOrWhiteSpace(manufacturer))
                throw new ArgumentException("Manufacturer cannot be null or empty", nameof(manufacturer));

            if (manufacturer.Length > 100)
                throw new ArgumentException("Manufacturer name cannot exceed 100 characters", nameof(manufacturer));

            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model cannot be null or empty", nameof(model));

            if (model.Length > 100)
                throw new ArgumentException("Model name cannot exceed 100 characters", nameof(model));

            if (year < 1886) // First car was made in 1886
                throw new ArgumentOutOfRangeException(nameof(year), "Year must be 1886 or later");

            if (startingBid <= 0)
                throw new ArgumentOutOfRangeException(nameof(startingBid), "Starting bid must be greater than zero");

            Id = id;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
            StartingBid = startingBid;
        }

        public abstract void DisplayInfo();
    }
}
