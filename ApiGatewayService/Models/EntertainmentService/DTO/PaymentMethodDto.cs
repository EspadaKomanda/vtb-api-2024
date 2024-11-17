using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.PaymentVariant;

namespace EntertaimentService.Models.PaymentMethod
{
    public class PaymentMethodEntertaimentDto
    {
        public long PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set;} = null!;
        public List<PaymentVariantEntertaimentDto>? PaymentVariants { get; set; }
    }
}