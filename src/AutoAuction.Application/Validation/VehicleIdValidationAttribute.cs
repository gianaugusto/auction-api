using System;
using System.ComponentModel.DataAnnotations;

namespace AutoAuction.Application.Validation
{
    public class VehicleIdValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var id = value as string;
            return !string.IsNullOrWhiteSpace(id) && id.Length <= 50;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a valid vehicle ID (max 50 characters).";
        }
    }
}
