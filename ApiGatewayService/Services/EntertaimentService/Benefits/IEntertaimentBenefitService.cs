using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.Benefits;
using EntertaimentService.Models.Benefits.Responses;

namespace ApiGatewayService.Services.EntertaimentService.Benefits
{
    public interface IEntertaimentBenefitService
    {
        public Task<AddBenefitResponse> AddBenefit(AddBenefitRequest request);
        public Task<GetBenefitsResponse> GetBenefits(GetBenefitsRequest request);
        public Task<GetBenefitResponse> GetBenefit(GetBenefitRequest request);
        public Task<UpdateBenefitResponse> UpdateBenefit(UpdateBenefitRequest request);
        public Task<RemoveBenefitResponse> RemoveBenefit(RemoveBenefitRequest request);
    }
}