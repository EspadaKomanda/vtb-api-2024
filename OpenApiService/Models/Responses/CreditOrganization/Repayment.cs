using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class Repayment
    {
        public string RepaymentType { get; set; }
        public List<string> Comments { get; set; }
        public string RepaymentFrequency { get; set; }
        public string AmountType { get; set; }
        public bool PaymentDate { get; set; }
        public OtherRepaymentType OtherRepaymentType { get; set; }
        public OtherRepaymentFrequency OtherRepaymentFrequency { get; set; }
        public OtherAmountType OtherAmountType { get; set; }
        public List<RepaymentFeeCharges> RepaymentFeeCharges { get; set; }
    }
}