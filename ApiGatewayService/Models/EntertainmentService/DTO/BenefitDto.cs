using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.Benefits
{
    public class BenefitEntertainmentDto
    {
        public long BenefitId { get; set; }
        public string BenefitName { get; set; } = null!;
    }
}