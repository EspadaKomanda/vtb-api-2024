using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Database.Models;
using EntertaimentService.Models.DTO;
using EntertaimentService.Models.Review.Requests;
using EntertaimentService.TourReview.Requests;
using TourService.Models.Review.Requests;

namespace EntertaimentService.Services.ReviewService
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