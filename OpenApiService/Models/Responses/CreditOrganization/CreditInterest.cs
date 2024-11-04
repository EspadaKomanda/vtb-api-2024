using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class CreditInterest
    {
         public List<string> Comments { get; set; }

    public List<TierBandSetDeposit> TierBandSetDeposit { get; set; }


    }
}