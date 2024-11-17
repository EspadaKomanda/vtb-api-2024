using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.PaymentVariant
{
    public class PaymentVariantEntertaimentDto
    {
        public long PaymentVariantId { get; set; }
        public long PaymentMethodId { get; set; }
        public string PaymentVariantName { get; set; } = null!;
        public double Price { get; set; }
    }
}