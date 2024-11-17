using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.PaymentVariant;

namespace ApiGatewayService.Models.TourService.Models.PaymentMethod
{
    public class PaymentMethodDto
    {
        public long PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set;} = null!;
        public List<PaymentVariantDto>? PaymentVariants { get; set; }
    }
}