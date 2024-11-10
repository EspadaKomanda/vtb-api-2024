using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TourService.Attributes.Validation;

namespace TourService.Models.PaymentVariant.Requests
{
    public class AddPaymentVariantRequest
    {
        [Required]
        public long PaymentMethodId { get; set; }
        [Required]
        [TourName]
        public string PaymentVariantName {get;set;} = null!;
        [Required]
        [TourPrice]
        public double Price {get;set;}
    }
}