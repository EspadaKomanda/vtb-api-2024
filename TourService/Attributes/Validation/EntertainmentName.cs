using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TourService.Attributes.Validation
{
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class EntertainmentName : ValidationAttribute
    {
        private static readonly string[] ProhibitedWords = { "admin", "administrator", "root", "sql", "drop", "select", "insert", "update", "delete" };

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            var entertainmentName = value.ToString()!;

            if (string.IsNullOrWhiteSpace(entertainmentName) || 
                !Regex.IsMatch(entertainmentName, @"^[a-zA-Z0-9\s\-]+$"))
            {
                return false;
            }

            foreach (var word in ProhibitedWords)
            {
                if (entertainmentName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return false;
                }
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"'{name}' is not a valid entertainment name.";
        }
    }
}