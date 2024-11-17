using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Models.TourService.Models.PaymentMethod.Requests
{
    public class UpdatePaymentMethodRequest
    {
        [Required]
        public long PaymentMethodId { get; set; }
        [Required]
        public string PaymentMethodName { get; set;} = null!;
    }
}