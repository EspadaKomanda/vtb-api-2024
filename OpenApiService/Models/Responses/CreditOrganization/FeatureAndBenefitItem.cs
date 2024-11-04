using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class FeatureAndBenefitItem
    {
        [Required]
        public Guid Identification { get; set; }
        public string Type { get; set; } = null!;
        public string Name { get; set; }  = null!;
        public List<string> Comments { get; set; } = null!;
        public string Amount { get; set; } = null!;
        public bool Indicator { get; set; }
        public string Textual { get; set; } = null!;
        public OtherType OtherType { get; set; }
        public List<FeatureBenefitEligibility> FeatureBenefitEligibility { get; set; } = null!;
    }
}