using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.Benefits.Responses
{
    public class GetBenefitsResponse
    {
        public List<BenefitEntertainmentDto>? Benefits { get; set; }
    }
}