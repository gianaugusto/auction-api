using System;
using System.ComponentModel.DataAnnotations;

namespace AutoAuction.Domain
{
    public enum VehicleType
    {
        [Display(Name = "Hatchback")]
        Hatchback,

        [Display(Name = "Sedan")]
        Sedan,

        [Display(Name = "SUV")]
        SUV,

        [Display(Name = "Truck")]
        Truck
    }
}
