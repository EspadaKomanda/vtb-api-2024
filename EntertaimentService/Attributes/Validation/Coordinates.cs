using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class Coordinates : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            var coordinateString = value.ToString()!;
            var parts = coordinateString.Split(',');

            if (parts.Length != 2)
            {
                return false;
            }

            if (double.TryParse(parts[0].Trim(), out double latitude) && 
                double.TryParse(parts[1].Trim(), out double longitude))
            {
                if (latitude < -90 || latitude > 90)
                {
                    return false;
                }

                if (longitude < -180 || longitude > 180)
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }

}