using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Models.PaymentMethod.Requests
{
    public class UpdatePaymentMethodRequest
    {
        [Required]
        public long PaymentMethodId { get; set; }
        [Required]
        [EntertaimentName]
        public string PaymentMethodName { get; set;} = null!;
    }
}