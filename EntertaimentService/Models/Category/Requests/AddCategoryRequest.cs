using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Models.Category.Requests
{
    public class AddCategoryRequest
    {
        [Required]
        [EntertaimentName]
        public string CategoryName { get; set; } = null!;
    }
}