using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Models.TourService.Models.Category
{
    public class CategoryDto
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}