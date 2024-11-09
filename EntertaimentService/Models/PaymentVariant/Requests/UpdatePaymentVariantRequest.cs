using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Models.PaymentVariant.Requests
{
    public class UpdatePaymentVariantRequest
    {
        [Required]
        public long PaymentVariantId { get; set; }
        [Required]
        public long PaymentMethodId { get; set; }
        [Required]
        [EntertaimentName]
        public string PaymentVariantName {get;set;} = null!;
        [Required]
        [EntertaimentPrice]
        public double Price {get;set;}
    }
}