using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TourService.Attributes.Validation;

namespace TourService.Models.PaymentMethod.Requests
{
    public class UpdatePaymentMethodRequest
    {
        [Required]
        public long PaymentMethodId { get; set; }
        [Required]
        [TourName]
        public string PaymentMethodName { get; set;} = null!;
    }
}