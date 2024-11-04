using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class OtherFeesCharges
    {
        [Required]
        public string TariffName { get; set; }
        public List<FeeChargeDetail> FeeChargeDetail { get; set; }
        public List<FeeChargeCap> FeeChargeCap { get; set; }
        public List<string> Comments { get; set; }
    }
}