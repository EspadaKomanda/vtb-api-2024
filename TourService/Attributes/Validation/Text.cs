using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Attributes.Validation
{
    using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class Text : ValidationAttribute
    {
        private static readonly string[] ProhibitedWords = 
        { 
               "admin", "administrator", "root" 
        };

        public override bool IsValid(object? value)
        {
            /*
            if (value == null)
            {
                return false;
            }

            var reviewText = value.ToString()!;

            if (string.IsNullOrWhiteSpace(reviewText))
            {
                return false;
            }

            if (!Regex.IsMatch(reviewText, @"^[\p{L}\p{N}\s\p{P}]+$"))
            {
                return false;
            }

            foreach (var word in ProhibitedWords)
            {
                if (reviewText.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return false;
                }
            }
            */
            return true;
        }

       
    }

}