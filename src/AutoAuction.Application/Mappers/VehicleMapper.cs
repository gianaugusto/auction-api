using AutoAuction.Domain;
using AutoAuction.Application.DTOs;
using System;

namespace AutoAuction.Application.Mappers
{
    public static class VehicleMapper
    {
        public static Vehicle ToDomain(this VehicleDto vehicleDto)
        {
            return vehicleDto.Type switch
            {
                "Hatchback" => new Hatchback(vehicleDto.Id, vehicleDto.Manufacturer, vehicleDto.Model, vehicleDto.Year, vehicleDto.StartingBid, vehicleDto.NumberOfDoors ?? 0),
                "Sedan" => new Sedan(vehicleDto.Id, vehicleDto.Manufacturer, vehicleDto.Model, vehicleDto.Year, vehicleDto.StartingBid, vehicleDto.NumberOfDoors ?? 0),
                "SUV" => new SUV(vehicleDto.Id, vehicleDto.Manufacturer, vehicleDto.Model, vehicleDto.Year, vehicleDto.StartingBid, vehicleDto.NumberOfSeats ?? 0),
                "Truck" => new Truck(vehicleDto.Id, vehicleDto.Manufacturer, vehicleDto.Model, vehicleDto.Year, vehicleDto.StartingBid, vehicleDto.LoadCapacity ?? 0m),
                _ => throw new ArgumentException("Invalid vehicle type")
            };
        }
    }
}
