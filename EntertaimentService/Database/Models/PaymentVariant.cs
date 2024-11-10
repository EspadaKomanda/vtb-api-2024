using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NJsonSchema.Annotations;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Database.Models
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
        [EntertaimentPrice]
        public double Price { get; set; }
    }
}