using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TourService.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class TourName : ValidationAttribute
    {
        private static readonly string[] ProhibitedWords = { "admin", "administrator", "root" };

        public override bool IsValid(object? value)
        {
            /*
            if (value == null)
            {
                return false;
            }

            var benefitName = value.ToString()!;

            if (string.IsNullOrWhiteSpace(benefitName) || 
                !Regex.IsMatch(benefitName, @"^[a-zA-Z0-9\s\-]+$"))
            {
                return false;
            }

            foreach (var word in ProhibitedWords)
            {
                if (benefitName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return false;
                }
            }
            */
            return true;
        }

    }
}