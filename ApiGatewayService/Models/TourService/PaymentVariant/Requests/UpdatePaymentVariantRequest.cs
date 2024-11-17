using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Models.TourService.Models.PaymentVariant.Requests
{
    public class UpdatePaymentVariantRequest
    {
        [Required]
        public long PaymentVariantId { get; set; }
        [Required]
        public long PaymentMethodId { get; set; }
        [Required]
        public string PaymentVariantName {get;set;} = null!;
        [Required]
        public double Price {get;set;}
    }
}