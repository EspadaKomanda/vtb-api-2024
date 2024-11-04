using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class OverdraftTierBandSet
    {
         public string EAR { get; set; }

    public List<string> Comments { get; set; }

    public string Identification { get; set; }

    public string TierCurrency { get; set; }

    public string TierValueMin { get; set; }

    public string TierValueMax { get; set; }

    public int AgreementLengthMin { get; set; }

    public int AgreementLengthMax { get; set; }

    public string AgreementPeriod { get; set; }

    public string OverdraftInterestChargingCoverage { get; set; }

    public bool BankGuaranteedIndicator { get; set; }

    public List<OverdraftFeesCharges> OverdraftFeesCharges { get; set; }


    }
}