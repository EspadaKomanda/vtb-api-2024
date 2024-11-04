using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class DebitInterestTierBand
    {
        public string AER { get; set; } = null!;
        public List<string> Comments { get; set; } = null!;
        public string Identification { get; set; } = null!;
        public string TierCurrency { get; set; } = null!;
        public string TierValueMinimum { get; set; } = null!;
        public string TierValueMaximum { get; set; } = null!;
        public int TierValueMinTerm { get; set; }
        public string MinTermPeriod { get; set; } = null!;
        public int TierValueMaxTerm { get; set; }
        public string MaxTermPeriod { get; set; } = null!;
        public string InterestRate { get; set; } = null!;
        public string InterestRateType { get; set; } = null!;
        public List<InterestFeesCharges> InterestFeesCharges { get; set; }
    }
}