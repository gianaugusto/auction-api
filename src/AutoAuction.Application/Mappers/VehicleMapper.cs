using AutoAuction.Domain;
using AutoAuction.Application.DTOs;
using System;
using AutoAuction.Domain.Exceptions;

namespace AutoAuction.Application.Mappers
{
    public static class VehicleMapper
    {
        public static Vehicle ToDomain(this VehicleDto vehicleDto)
        {
            if (vehicleDto == null)
                throw new ArgumentNullException(nameof(vehicleDto));

            return vehicleDto.Type switch
            {
                VehicleType.Hatchback => new Hatchback(vehicleDto.Id, vehicleDto.Manufacturer, vehicleDto.Model, vehicleDto.Year, vehicleDto.StartingBid, vehicleDto.NumberOfDoors ?? 0),
                VehicleType.Sedan => new Sedan(vehicleDto.Id, vehicleDto.Manufacturer, vehicleDto.Model, vehicleDto.Year, vehicleDto.StartingBid, vehicleDto.NumberOfDoors ?? 0),
                VehicleType.SUV => new SUV(vehicleDto.Id, vehicleDto.Manufacturer, vehicleDto.Model, vehicleDto.Year, vehicleDto.StartingBid, vehicleDto.NumberOfSeats ?? 0),
                VehicleType.Truck => new Truck(vehicleDto.Id, vehicleDto.Manufacturer, vehicleDto.Model, vehicleDto.Year, vehicleDto.StartingBid, vehicleDto.LoadCapacity ?? 0m),
                _ => throw new ArgumentException("Invalid vehicle type")
            };
        }
    }
}
