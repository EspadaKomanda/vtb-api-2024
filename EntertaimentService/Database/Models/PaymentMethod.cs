using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Database.Models
{
    public class PaymentMethod
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [EntertaimentName]
        public string Name { get; set; } = null!;
        public ICollection<PaymentVariant>? PaymentVariants { get; set; } 
    }
}