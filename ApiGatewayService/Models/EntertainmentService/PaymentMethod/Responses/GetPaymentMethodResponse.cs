using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.PaymentMethod.Responses
{
    public class GetPaymentMethodEntertaimentResponse
    {
        public PaymentMethodEntertaimentDto PaymentMethod { get; set; } = null!;
    }
}