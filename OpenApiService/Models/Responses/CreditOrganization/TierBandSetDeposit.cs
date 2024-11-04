using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class TierBandSetDeposit
    {
        public string TierBandMethodDeposit { get; set; }
        public string TierBandMethod { get; set; }
        public string CalculationMethod { get; set; }
        public List<Other> OtherCalculationMethod { get; set; }
        public string WithdrawInterest { get; set; }
        public string CapitalizationIdentification { get; set; }
        public string CapitalizationFrequency { get; set; }
        public string EarlyTermination { get; set; }
        public string Destination { get; set; }
        public List<string> Comments { get; set; }
        public List<TierBandDeposit> TierBandDeposit { get; set; }
        public List<TierBandSetEligibility> TierBandSetEligibility { get; set; }
    }
}