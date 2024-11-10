using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TourService.Attributes.Validation;
using TourService.Database.Models;
using TourService.Models.PaymentVariant;

namespace TourService.Models.PaymentMethod.Requests
{
    public class AddPaymentMethodRequest
    {
        [Required]
        [TourName]
        public string PaymentMethodName { get; set; } = null!;
        public List<PaymentVariantDto>? PaymentVariants { get; set; }
    }
}