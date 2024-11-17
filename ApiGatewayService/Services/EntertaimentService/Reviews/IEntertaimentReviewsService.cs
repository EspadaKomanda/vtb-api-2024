using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.Review.Requests;
using EntertaimentService.Models.Review.Responses;
using EntertaimentService.TourReview.Requests;

namespace ApiGatewayService.Services.EntertaimentService.Reviews
{
    public interface IEntertaimentReviewsService
    {
        Task<AddReviewResponse> AddReview(AddReviewEntertaimentRequest request);
        Task<UpdateReviewResponse> UpdateReview(UpdateReviewEntertainmentRequest request);
        Task<GetReviewResponse> GetReview(GetReviewEntertainmentRequest request);
        Task<GetReviewsResponse> GetReviews(GetReviewsEntertaimentRequest request);
        Task<RemoveReviewResponse> RemoveReview(RemoveReviewEntertainmentRequest request);
    }
}