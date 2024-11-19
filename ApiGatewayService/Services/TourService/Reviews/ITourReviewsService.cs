using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.Review.Requests;
using ApiGatewayService.Models.TourService.Models.Review.Responses;
using ApiGatewayService.Models.TourService.TourReview.Requests;
using TourService.Models.Review.Responses;

namespace ApiGatewayService.Services.TourService.Reviews
{
    public interface ITourReviewsService
    {
        Task<AddReviewResponse> AddReview(AddReviewRequest request);
        Task<UpdateReviewResponse> UpdateReview(UpdateReviewRequest request);
        Task<GetReviewResponse> GetReview(GetReviewRequest request);
        Task<GetReviewsResponse> GetReviews(GetReviewsRequest request);
        Task<RemoveReviewResponse> RemoveReview(RemoveReviewRequest request);
    }
}