using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.Tour.Requests
{
    public class LinkPaymentMethodsRequest
    {
        [Required]
        public long TourId { get; set; }
        [Required]
        public List<long> PaymentMethods { get; set; } = null!;
    }
}