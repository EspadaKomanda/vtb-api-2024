using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.Tour.Requests
{
    public class UnlinkCategoryEntertaimentRequest
    {
        [Required]
        public long EntertaimentId { get; set; }
        [Required]
        public long CategoryId { get; set; }
    }
}