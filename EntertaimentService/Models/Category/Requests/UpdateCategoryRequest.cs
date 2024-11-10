using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Models.Category.Requests
{
    public class UpdateCategoryRequest
    {
        [Required]
        public long CategoryId { get; set; }
        [Required]
        [EntertaimentName]
        public string CategoryName { get; set;} = null!;
    }
}