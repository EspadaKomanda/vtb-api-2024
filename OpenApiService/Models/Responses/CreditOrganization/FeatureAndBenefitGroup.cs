using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class FeatureAndBenefitGroup
    {
        [Required]
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public List<string> Comments { get; set; } = null!;
        public string BenefitGroupNominalValue { get; set; } = null!;
        public string Fee { get; set; } = null!;
        public string ApplicationFrequency { get; set; } = null!;
        public OtherApplicationFrequency OtherApplicationFrequency { get; set; }
        public string CalculationFrequency { get; set; } = null!;
        public OtherCalculationFrequency OtherCalculationFrequency { get; set; }
        public OtherType OtherType { get; set; } 
        public List<FeatureBenefitEligibility> FeatureBenefitEligibility { get; set; }  = null!;
    }
}