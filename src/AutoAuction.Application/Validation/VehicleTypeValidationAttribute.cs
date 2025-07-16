using System;
using System.ComponentModel.DataAnnotations;
using AutoAuction.Domain;

namespace AutoAuction.Application.Validation
{
    public class VehicleTypeValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            return Enum.IsDefined(typeof(VehicleType), value);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} is not a valid vehicle type.";
        }
    }
}
