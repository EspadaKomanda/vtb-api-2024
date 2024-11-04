using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class RepaymentFeeCharges
    {
        public List<FeeChargeDetail> FeeChargeDetail { get; set; }
        public List<FeeChargeCap> FeeChargeCap { get; set; }
    }
}