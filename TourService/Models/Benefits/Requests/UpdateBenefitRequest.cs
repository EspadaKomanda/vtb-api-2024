using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TourService.Attributes.Validation;

namespace TourService.Models.Benefits
{
    public class UpdateBenefitRequest
    {
        [Required]
        public long BenefitId { get; set; }
        [Required]
        [TourName]
        public string BenefitName { get; set;} = null!;
    }
}