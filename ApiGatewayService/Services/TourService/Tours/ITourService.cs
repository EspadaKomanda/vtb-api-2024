using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Models.Tour.Requests;
using TourService.Models.Tour.Responses;

namespace ApiGatewayService.Services.TourService.Tours
{
    public interface ITourService
    {
        Task<AddTourResponse> AddTour(AddTourRequest addTourRequest);
        Task<GetTourResponse> GetTour(GetTourRequest getTourRequest);
        Task<UpdateTourResponse> UpdateTour(UpdateTourRequest updateTourRequest);
        Task<RemoveTourResponse> RemoveTour(RemoveTourRequest removeTourRequest);
        Task<GetToursResponse> GetTours(GetToursRequest getToursRequest);
        Task<LinkCategoriesResponse> LinkCategories(LinkCategoriesRequests linkCategoriesRequest);
        Task<UnlinkResponse> UnlinkCategory(UnlinkCategoryRequest unlinkCategoryRequest);
        Task<LinkPaymentsResponse> LinkPaymentMethods(LinkPaymentMethodsRequest linkPaymentMethodsRequest);
        Task<UnlinkResponse> UnlinkPaymentMethod(UnlinkPaymentMethodRequest unlinkPaymentMethodRequest);
        Task<LinkTagsResponse> LinkTags(LinkTagsRequest linkTagsRequest);
        Task<UnlinkResponse> UnlinkTag(UnlinkTagRequest unlinkTagRequest);

    }
}