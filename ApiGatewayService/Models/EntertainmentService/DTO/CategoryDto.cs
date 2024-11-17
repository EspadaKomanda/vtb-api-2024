using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.Category
{
    public class CategoryEntertaimentDto
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}