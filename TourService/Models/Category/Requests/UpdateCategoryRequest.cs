using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TourService.Attributes.Validation;

namespace TourService.Models.Category.Requests
{
    public class UpdateCategoryRequest
    {
        [Required]
        public long CategoryId { get; set; }
        [Required]
        [TourName]
        public string CategoryName { get; set;} = null!;
    }
}