using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TourService.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class Description : ValidationAttribute
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

            var description = value.ToString()!;

            if (string.IsNullOrWhiteSpace(description))
            {
                return false;
            }

            if (!Regex.IsMatch(description, @"^[а-яА-ЯёЁa-zA-Z0-9\s\.\,\!\?\""\-]+$"))
            {
                return false;
            }

            foreach (var word in ProhibitedWords)
            {
                if (description.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return false;
                }
            }

            return true;
        }

    }
}