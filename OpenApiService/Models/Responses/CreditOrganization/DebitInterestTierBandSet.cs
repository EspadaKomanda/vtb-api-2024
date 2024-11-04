using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class DebitInterestTierBandSet
    {
            public string Identification { get; set; }

    public string TierBandMethod { get; set; }

    public string CalculationMethod { get; set; }

    public List<string> Comments { get; set; }

    public List<DebitInterestTierBand> DebitInterestTierBand { get; set; }


    }
}