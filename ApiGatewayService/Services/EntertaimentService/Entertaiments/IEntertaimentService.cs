using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.Entertaiment.Requests;
using EntertaimentService.Models.Tour.Requests;
using EntertaimentService.Models.Tour.Responses;
using EntertainmentService.Models.Tour.Responses;

namespace ApiGatewayService.Services.EntertaimentService.Entertaiments
{
    public interface IEntertaimentService
    {
        Task<AddEntertaimentResponse> AddEntertaiment(AddEntertaimentRequest entertaimentRequest);
        Task<UpdateEntertaimentResponse> UpdateEntertaiment(UpdateEntertaimentRequest entertaimentRequest);
        Task<RemoveEntertaimentResponse> RemoveEntertaiment(RemoveEntertaimentRequest entertaimentRequest);
        Task<UnlinkResponse> UnlinkCategory(UnlinkCategoryEntertaimentRequest entertaimentRequest);
        Task<UnlinkResponse> UnlinkPaymentMethod(UnlinkPaymentMethodEntertaimentRequest entertaimentRequest);
        Task<GetEntertaimentResponse> GetEntertaiment(GetEntertaimentRequest entertaimentRequest);
        Task<GetEntertaimentsResponse> GetEntertaiments(GetEntertaimentsRequest entertaimentRequest);
        Task<LinkCategoriesResponse> LinkCategories(LinkCategoriesEntertaimentRequests entertaimentRequest);
        Task<LinkPaymentsResponse> LinkPaymentMethods(LinkPaymentMethodsEntertaimentRequest entertaimentRequest);
        
    }
}