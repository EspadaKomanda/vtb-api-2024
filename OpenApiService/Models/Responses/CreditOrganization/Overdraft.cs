using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class Overdraft
    {
        public List<string> Comments { get; set; }
        public List<OverdraftTierBandSet> OverdraftTierBandSet { get; set; }
    }
}