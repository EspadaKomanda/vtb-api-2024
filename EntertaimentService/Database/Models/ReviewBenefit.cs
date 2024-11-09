using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Database.Models
{
    public class ReviewBenefit
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("RewiewId")]
        public long ReviewId { get; set; }
        [Required]
        public Review Review { get;set; } = null!;
        [ForeignKey("BenefitId")]
        public long BenefitId { get; set; }
        [Required]
        public Benefit Benefit { get; set; } = null!;
    }
}