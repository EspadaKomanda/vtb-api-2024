using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Models.TourService.Models.Benefits
{
    public class GetBenefitsRequest
    {
        [Required]
        public int Page {get; set;}
    }
}