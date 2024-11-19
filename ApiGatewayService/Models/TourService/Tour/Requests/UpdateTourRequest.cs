using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.Tour.Requests
{
    public class UpdateTourRequest
    {
        [Required]
        public long TourId { get; set; }
        
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public double Price { get; set; }

        [Required]
        public string Address { get; set; } = null!;

        public string? Coordinates { get; set; }

        [Required]
        public bool IsActive { get; set; }

    }
}