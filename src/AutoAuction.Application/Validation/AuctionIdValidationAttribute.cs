using System;
using System.ComponentModel.DataAnnotations;

namespace AutoAuction.Application.Validation
{
    public class AuctionIdValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var id = (int)value;
            return id > 0;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a valid auction ID (greater than zero).";
        }
    }
}
