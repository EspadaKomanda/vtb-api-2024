using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.Tour.Requests
{
    public class LinkCategoriesRequests
    {
        [Required]
        public long EntertaimentId { get; set; }
        public List<long> Categories { get; set; } = null!;
    }
}