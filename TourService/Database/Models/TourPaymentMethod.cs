using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Database.Models
{
    public class TourPaymentMethod
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("TourId")]
        public long TourId { get; set; }
        [Required]
        public Tour Tour { get; set; } = null!;
        [ForeignKey("PaymentMethodId")]
        public long PaymentMethodId { get; set; }
        [Required]
        public PaymentMethod PaymentMethod{ get; set; } = null!;
    }
}