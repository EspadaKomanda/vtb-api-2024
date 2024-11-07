using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.Tour.Requests
{
    public class LinkCategoriesRequests
    {
        public List<long> Categories { get; set; } = null!;
    }
}