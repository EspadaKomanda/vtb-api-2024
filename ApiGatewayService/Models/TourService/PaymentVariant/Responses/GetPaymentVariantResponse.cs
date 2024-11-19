using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Models.TourService.Models.PaymentVariant.Responses
{
    public class GetPaymentVariantResponse
    {
        public PaymentVariantDto PaymentVariant { get; set; } = null!;
    }
}