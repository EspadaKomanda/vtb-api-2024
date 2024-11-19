using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.Category.Responses
{
    public class GetCategoriesResponse
    {
        public List<CategoryEntertaimentDto>? Categories { get; set; }
    }
}