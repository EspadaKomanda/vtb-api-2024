using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class Insurance
    {
        [Required]
        public string InsuranceId { get; set; } = null!;
        [Required]
        public string InsuranceName { get; set; } = null!;
        public List<string> Comments { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string AmountRate { get; set; } = null!;
        [Url]
        public string AgreementUrl { get; set; } = null!;
        public List<string> RiskComments { get; set; } = null!;
    }
}