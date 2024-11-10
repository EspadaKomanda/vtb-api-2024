using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.Category.Requests
{
    public class GetCategoryRequest
    {
        [Required]
        public long CategoryId { get; set; }
    }
}