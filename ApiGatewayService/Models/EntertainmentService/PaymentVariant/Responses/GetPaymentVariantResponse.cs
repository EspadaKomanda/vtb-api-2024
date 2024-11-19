using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.PaymentVariant.Responses
{
    public class GetPaymentVariantResponse
    {
        public PaymentVariantEntertaimentDto PaymentVariant { get; set; } = null!;
    }
}