using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.Tour.Requests
{
    public class LinkCategoriesRequests
    {
        [Required]
        public long TourId { get; set; }
        public List<long> Categories { get; set; } = null!;
    }
}