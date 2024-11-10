using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TourService.Attributes.Validation;

namespace TourService.Database.Models
{
    public class PaymentMethod
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [TourName]
        public string Name { get; set; } = null!;
        public ICollection<PaymentVariant>? PaymentVariants { get; set; } 
    }
}