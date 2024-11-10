using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Models.Entertaiment.Requests
{
    public class UpdateEntertaimentRequest
    {
        [Required]
        public long EntertaimentId { get; set; }
        
        [Required]
        [EntertaimentName]
        public string Name { get; set; } = null!;

        [Required]
        [EntertaimentDescription]
        public string Description { get; set; } = null!;

        [Required]
        [EntertaimentPrice]
        public double Price { get; set; }

        [Required]
        [EntertaimentAddress]
        public string Address { get; set; } = null!;

        [Coordinates]
        public string? Coordinates { get; set; }

        [Required]
        public bool IsActive { get; set; }

    }
}