using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Models.TourService.Models.Category.Responses
{
    public class GetCategoriesResponse
    {
        public List<CategoryDto>? Categories { get; set; }
    }
}