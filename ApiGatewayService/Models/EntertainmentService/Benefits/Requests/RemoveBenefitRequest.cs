using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.Benefits
{
    public class RemoveBenefitRequest
    {
        [Required]
        public long BenefitId { get; set;}
    }
}