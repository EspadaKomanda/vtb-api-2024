using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.PaymentVariant.Requests
{
    public class RemovePaymentVariantRequest
    {
        [Required]
        public long PaymentVariantId { get; set; }
    }
}