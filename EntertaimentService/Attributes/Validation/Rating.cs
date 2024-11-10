using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class Rating : ValidationAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public Rating(int min = 1, int max = 5)
        {
            _min = min;
            _max = max;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is int rating)
            {
                return rating >= _min && rating <= _max;
            }

            return false;
        }

       
    }
}