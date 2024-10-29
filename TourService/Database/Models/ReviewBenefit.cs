using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Database.Models
{
    public class ReviewBenefit
    {
        //TODO: Add validation attributes
        [Key]
        public long Id { get; set; }
        [ForeignKey("RewiewId")]
        public long ReviewId { get; set; }
        public Review Review { get;set; }
        [ForeignKey("BenefitId")]
        public long BenefitId { get; set; }
        public Benefit Benefit { get; set; }
    }
}