using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class OverdraftFeeChargeDetail
    {
        public string FeeType { get; set; }
        public OtherFeeType OtherFeeType { get; set; }
        public string FeeAmount { get; set; }
        public string FeeRate { get; set; }
        public string FeeRateType { get; set; }
        public OtherFeeRateType OtherFeeRateType { get; set; }
        public string ApplicationFrequency { get; set; }
        public OtherApplicationFrequency OtherApplicationFrequency { get; set; }
        public string CalculationFrequency { get; set; }
        public OtherCalculationFrequency OtherCalculationFrequency { get; set; }
        public string IncrementalBorrowingAmount { get; set; }
        public bool NegotiableIndicator { get; set; }
        public List<string> Comments { get; set; }
        public List<FeeChargeCap> FeeChargeCap { get; set; }
    }
}