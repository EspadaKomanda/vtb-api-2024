using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Models.TourService.Models.Benefits
{
    public class UpdateBenefitRequest
    {
        [Required]
        public long BenefitId { get; set; }
        [Required]
        public string BenefitName { get; set;} = null!;
    }
}