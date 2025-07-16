using System;
using System.ComponentModel.DataAnnotations;

namespace AutoAuction.Application.Validation
{
    public class BidAmountValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var bidAmount = (decimal)value;
            return bidAmount > 0;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be greater than zero.";
        }
    }
}
