using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.Tag.Requests
{
    public class GetTagRequest
    {
        [Required]
        public long TagId { get; set; }
    }
}