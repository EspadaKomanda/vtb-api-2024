using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Database.Models;
using TourService.Models.DTO;
using TourService.Models.Tour.Requests;

namespace TourService.Services.TourService
{
    public interface ITourService
    {
        Task<long> AddTour(Tour tour);
        Task<TourDto> GetTour(GetTourRequest request);
        IQueryable<TourDto> GetTours(GetToursRequest getTours);
        bool UpdateTour(UpdateTourRequest updateTour);
        bool RemoveTour(RemoveTourRequest removeTour);
        Task<bool> LinkTags(LinkTagsRequest linkTags);
        bool UnlinkTag(UnlinkTagRequest unlinkTag);
        Task<bool> LinkCategories(LinkCategoriesRequests linkCategories);
        bool UnlinkCategory(UnlinkCategoryRequest unlinkCategory);
        Task<bool> LinkPaymentMethods(LinkPaymentMethodsRequest linkPaymentMethods);
        bool UnlinkPaymentMethod(UnlinkPaymentMethodRequest unlinkPaymentMethod);

    }
}