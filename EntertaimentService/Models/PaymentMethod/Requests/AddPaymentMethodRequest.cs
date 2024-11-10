using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Attributes.Validation;
using EntertaimentService.Database.Models;
using EntertaimentService.Models.PaymentVariant;

namespace EntertaimentService.Models.PaymentMethod.Requests
{
    public class AddPaymentMethodRequest
    {
        [Required]
        [EntertaimentName]
        public string PaymentMethodName { get; set; } = null!;
        public List<PaymentVariantDto>? PaymentVariants { get; set; }
    }
}