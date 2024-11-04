using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class FeatureBenefitEligibility
    {
        public string Name { get; set; }

    public string Description { get; set; }

    public string Type { get; set; }

    public List<string> Comments { get; set; }

    public string Amount { get; set; }

    public bool Indicator { get; set; }

    public string Textual { get; set; }

    public string Period { get; set; }

    public List<OtherEligibilityType> OtherEligibilityType { get; set; }


    }
}