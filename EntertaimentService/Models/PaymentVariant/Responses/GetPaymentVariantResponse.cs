using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.PaymentVariant.Responses
{
    public class GetPaymentVariantResponse
    {
        public PaymentVariantDto PaymentVariant { get; set; } = null!;
    }
}