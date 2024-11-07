using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NJsonSchema.Annotations;
using TourService.Attributes.Validation;

namespace TourService.Database.Models
{
    public class PaymentVariant
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("PaymentMethodId")]
        public long PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod {get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        [TourPrice]
        public double Price { get; set; }
    }
}