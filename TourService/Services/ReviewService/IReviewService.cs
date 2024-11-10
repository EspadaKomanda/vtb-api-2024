using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Database.Models;
using TourService.Models.DTO;
using TourService.Models.Review.Requests;
using TourService.TourReview.Requests;

namespace TourService.Services.ReviewService
{
    public interface IReviewService
    {
        Task<long> AddReview(AddReviewRequest review);
        Task<ReviewDto> GetReview(GetReviewRequest getReview);
        IQueryable<ReviewDto> GetReviews(GetReviewsRequest getReviews);
        Task<bool> RateReview(RateReviewRequest rateReview);
        bool RemoveReview(RemoveReviewRequest removeReview);
        bool UpdateReview(UpdateReviewRequest updateReview);
    }
}