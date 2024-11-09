using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.PaymentVariant.Requests
{
    public class GetPaymentVariantsRequest
    {
        [Required]
        public long PaymentMethodId { get; set; }
    }
}