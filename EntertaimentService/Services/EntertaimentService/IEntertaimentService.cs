using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Database.Models;
using EntertaimentService.Models.DTO;
using EntertaimentService.Models.Tour.Requests;

namespace EntertaimentService.Services.EntertaimentServices
{
    public interface IEntertaimentService
    {
        Task<long> AddEntertaiment(Entertaiment tour);
        Task<EntertaimentDto> GetEntertaiment(GetEntertaimentRequest request);
        IQueryable<EntertaimentDto> GetEntertaiments(GetEntertaimentsRequest getEntertaiments);
        bool UpdateEntertaiment(UpdateEntertaimentRequest updateEntertaiment);
        bool RemoveEntertaiment(RemoveEntertaimentRequest removeEntertaiment);
        Task<bool> LinkCategories(LinkCategoriesRequests linkCategories);
        bool UnlinkCategory(UnlinkCategoryRequest unlinkCategory);
        Task<bool> LinkPaymentMethods(LinkPaymentMethodsRequest linkPaymentMethods);
        bool UnlinkPaymentMethod(UnlinkPaymentMethodRequest unlinkPaymentMethod);

    }
}