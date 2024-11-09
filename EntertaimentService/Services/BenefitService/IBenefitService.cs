using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Database.Models;
using EntertaimentService.Models.Benefits;

namespace EntertaimentService.Services.BenefitService
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