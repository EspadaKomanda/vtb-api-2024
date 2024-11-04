using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
   public class FeatureAndBenefit

{

    public List<FeatureAndBenefitGroup> FeatureAndBenefitGroup { get; set; }

    public List<FeatureAndBenefitItem> FeatureAndBenefitItem { get; set; }

}
}