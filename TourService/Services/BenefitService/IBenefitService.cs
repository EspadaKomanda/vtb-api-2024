using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Database.Models;
using TourService.Models.Benefits;

namespace TourService.Services.BenefitService
{
    public interface IBenefitService
    {
        Task<long> AddBenefit(Benefit benefit);
        Task<BenefitDto> GetBenefit(GetBenefitRequest getBenefit);
        IQueryable<BenefitDto> GetBenefits(GetBenefitsRequest getBenefits);
        bool RemoveBenefit(RemoveBenefitRequest removeBenefit);
        bool UpdateBenefit(UpdateBenefitRequest updateBenefit);
    }
}