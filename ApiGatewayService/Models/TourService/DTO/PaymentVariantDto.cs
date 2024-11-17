using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Models.TourService.Models.PaymentVariant
{
    public class PaymentVariantDto
    {
        public long PaymentVariantId { get; set; }
        public long PaymentMethodId { get; set; }
        public string PaymentVariantName { get; set; } = null!;
        public double Price { get; set; }
    }
}