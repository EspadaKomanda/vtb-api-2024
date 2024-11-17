using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Models.ourService.Models.Benefits;
using ApiGatewayService.Models.TourService.Models.Benefits;
using ApiGatewayService.Models.TourService.Models.Benefits.Responses;

namespace ApiGatewayService.Services.TourService.Benefits
{
    public interface ITourBenefitsService
    {
        public Task<AddBenefitResponse> AddBenefit(AddBenefitRequest request);
        public Task<GetBenefitsResponse> GetBenefits(GetBenefitsRequest request);
        public Task<GetBenefitResponse> GetBenefit(GetBenefitRequest request);
        public Task<UpdateBenefitResponse> UpdateBenefit(UpdateBenefitRequest request);
        public Task<RemoveBenefitResponse> RemoveBenefit(RemoveBenefitRequest request);
    }
}