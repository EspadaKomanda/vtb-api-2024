using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.PaymentVariant;

namespace EntertaimentService.Models.PaymentMethod.Requests
{
    public class AddPaymentMethodEntertaimentRequest
    {
        [Required]
        public string PaymentMethodName { get; set; } = null!;
        public List<PaymentVariantEntertaimentDto>? PaymentVariants { get; set; }
    }
}