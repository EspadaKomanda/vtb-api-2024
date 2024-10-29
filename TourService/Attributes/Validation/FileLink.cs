using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TourService.Attributes.Validation
{
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class FileLink : ValidationAttribute
    {
        private static readonly string[] ProhibitedWords = { "admin", "administrator", "root", "sql", "drop", "select", "insert", "update", "delete" };

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            var fileLink = value.ToString()!;

            if (string.IsNullOrWhiteSpace(fileLink) || 
                !Regex.IsMatch(fileLink, @"^(http|https)://[^\s/$.?#].[^\s]*$"))
            {
                return false;
            }

            foreach (var word in ProhibitedWords)
            {
                if (fileLink.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return false;
                }
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"'{name}' is not a valid file link.";
        }
    }
}