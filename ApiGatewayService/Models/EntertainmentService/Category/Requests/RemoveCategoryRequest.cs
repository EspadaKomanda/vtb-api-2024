using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.Category.Requests
{
    public class RemoveCategoryRequest
    {
        public long CategoryId { get; set; }
    }
}