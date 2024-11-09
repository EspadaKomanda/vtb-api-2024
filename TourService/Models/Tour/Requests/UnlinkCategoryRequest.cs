using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.Tour.Requests
{
    public class UnlinkCategoryRequest
    {
        [Required]
        public long TourId { get; set; }
        [Required]
        public long CategoryId { get; set; }
    }
}