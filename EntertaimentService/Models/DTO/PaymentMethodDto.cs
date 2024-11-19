using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.PaymentVariant;
using TourService.Models.PaymentVariant;

namespace TourService.Models.PaymentMethod
{
    public class PaymentMethodDto
    {
        public long PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set;} = null!;
        public List<PaymentVariantDto>? PaymentVariants { get; set; }
    }
}