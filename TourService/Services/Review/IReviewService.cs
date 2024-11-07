using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Models.Review.Requests;
using TourService.Models.Review.Responses;
using TourService.TourReview.Requests;

namespace TourService.Services.Review
{
    public interface IReviewService
    {
        Task<DeleteReviewResponse> DeleteReview(DeleteReviewRequest deleteReviewRequest);
        Task<GetReviewResponse> GetReview(GetReviewRequest getReviewRequest);
        Task<GetReviewsResponse> GetReviews(GetReviewsRequest getReviewsRequest);
        Task<PostReviewResponse> PostReview(PostReviewRequest postReviewRequest);
        Task<RateReviewResponse> RateReview(RateReviewRequest rateReviewRequest);
    }
}