using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.PaymentMethod.Responses
{
    public class GetPaymentMethodResponse
    {
        public PaymentMethodDto PaymentMethod { get; set; } = null!;
    }
}