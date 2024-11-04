using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class TierBandDeposit
    {
        public string AER { get; set; }

    public string RateIncrease { get; set; }

    public string RateDecrease { get; set; }

    public string ProlongationType { get; set; }

    public string MaxAutoprolongation { get; set; }

    public bool Replenishment { get; set; }

    public string ReplenishmentFrequency { get; set; }

    public string MaxAmountDeposit { get; set; }

    public string Restriction { get; set; }

    public string RestrictionType { get; set; }

    public List<string> Comments { get; set; }

    public string Identification { get; set; }

    public string TierCurrency { get; set; }

    public string InterestRate { get; set; }

    public string TierValueMinimum { get; set; }

    public string TierValueMaximum { get; set; }

    public string TierPrimaryValueMinimum { get; set; }

    public string TierPrimaryValueMaximum { get; set; }

    public string TierRemainValueMinimum { get; set; }

    public int TierValueMinTerm { get; set; }

    public string MinTermPeriod { get; set; }

    public int TierValueMaxTerm { get; set; }

    public string MaxTermPeriod { get; set; }

    public string NonLowerBalanceValue { get; set; }

    public string NonLowerBalance { get; set; }

    public string ApplicationFrequency { get; set; }

    public List<OtherApplicationFrequency> OtherApplicationFrequency { get; set; }

    public string CalculationFrequency { get; set; }

    public List<OtherCalculationFrequency> OtherCalculationFrequency { get; set; }

    public List<InterestFeesCharges> InterestFeesCharges { get; set; }


    }
}