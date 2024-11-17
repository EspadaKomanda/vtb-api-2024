using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.Tour.Requests
{
    public class UnlinkPaymentMethodRequest
    {
        [Required]
        public long EntertaimentId { get; set; }
        [Required]
        public long PaymentMethodId { get; set; }
    }
}