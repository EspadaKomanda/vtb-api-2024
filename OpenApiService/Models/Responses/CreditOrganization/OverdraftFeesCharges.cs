using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class OverdraftFeesCharges
    {
        public List<OverdraftFeeChargeCap> OverdraftFeeChargeCap { get; set; }
        public List<OverdraftFeeChargeDetail> OverdraftFeeChargeDetail { get; set; }
    }
}