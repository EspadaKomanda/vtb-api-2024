using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.Benefits.Responses
{
    public class GetBenefitsResponse
    {
        public List<BenefitDto>? Benefits { get; set; }
    }
}