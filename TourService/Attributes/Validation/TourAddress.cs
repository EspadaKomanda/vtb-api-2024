using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TourService.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class TourAddress : ValidationAttribute
    {
        private static readonly string[] ProhibitedWords = 
        { 
            "admin", "administrator", "root" 
        };

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            var address = value.ToString()!;

            if (string.IsNullOrWhiteSpace(address))
            {
                return false;
            }

            var addressPattern = @"^\d+\s[A-Za-z0-9\s.,'-]+(?:\s(Apt|Suite|Unit|#)\s?\d+[A-Za-z]?)?,?\s?[A-Za-z\s]+(?:,\s?[A-Za-z]{2})?\s?\d{5}(-\d{4})?$";
            if (!Regex.IsMatch(address, addressPattern))
            {
                return false;
            }

            foreach (var word in ProhibitedWords)
            {
                if (address.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return false;
                }
            }

           
            if (address.Length < 5) 
            {
                return false;
            }

            return true;
        }
    }
}