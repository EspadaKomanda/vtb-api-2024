using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.PaymentVariant;

namespace ApiGatewayService.Models.TourService.Models.PaymentMethod.Requests
{
    public class AddPaymentMethodRequest
    {
        [Required]
        public string PaymentMethodName { get; set; } = null!;
        public List<PaymentVariantDto>? PaymentVariants { get; set; }
    }
}