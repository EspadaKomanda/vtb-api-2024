using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class OverdraftFeeChargeCap
    {
        public string FeeType { get; set; }
        public string MinMaxType { get; set; }
        public int FeeCapOccurrence { get; set; }
        public string FeeCapAmount { get; set; }
        public string CappingPeriod { get; set; }
        public List<string> Comments { get; set; }
    }
}