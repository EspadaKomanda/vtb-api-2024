using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.PaymentMethod.Requests
{
    public class RemovePaymentMethodRequest
    {
        [Required]
        public long PaymentMethodId { get; set; }
    }
}