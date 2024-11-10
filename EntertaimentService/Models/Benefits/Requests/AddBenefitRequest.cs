using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Models.Benefits
{
    public class AddBenefitRequest
    {
        [Required]
        [EntertaimentName]
        public string BenefitName {get; set;} = null!;
    }
}