using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using EntertaimentService.Database.Models;
using EntertaimentService.Exceptions.Database;
using EntertaimentService.Kafka;
using EntertaimentService.KafkaException.ConsumerException;
using EntertaimentService.Models.Benefits;
using EntertaimentService.Models.DTO;
using EntertaimentService.Models.Review.Requests;
using EntertaimentService.Models.User.Requests;
using EntertaimentService.TourReview.Requests;
using UserService.Repositories;
using TourService.Models.User.Responses;
using TourService.Models.DTO;
using TourService.Models.Review.Requests;
using TourService.Kafka;

namespace EntertaimentService.Services.ReviewService
{
    public class ReviewService(IUnitOfWork unitOfWork, ILogger<ReviewService> logger, IMapper mapper, KafkaRequestService kafkaRequestService) : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<ReviewService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly KafkaRequestService _kafkaRequestService = kafkaRequestService;
        
        public async Task<long> AddReview(AddReviewRequest review)
        {
            using var transaction = _unitOfWork.BeginTransaction();
            
            try
            {
                Review currentReview = new Review(){
                    EntertaimentId = review.EntertaimentId,
                    UserId = review.UserId,
                    Text = review.Text,
                    Rating = review.Rating,
                    IsAnonymous = review.IsAnonymous,
                    Date = DateTime.Now,
                    Removed = false,
                };

                var result = await _unitOfWork.Reviews.AddAsync(currentReview);
                if(review.Benefits != null)
                {
                    foreach(var benefit in review.Benefits)
                    {
                        await _unitOfWork.ReviewBenefits.AddAsync(new ReviewBenefit(){
                            ReviewId = result.Id,
                            BenefitId = benefit
                        });
                    }
                }
                
                if(transaction.SaveAndCommit())
                {
                    _logger.LogDebug("Successefully added review");
                    EntertaimentService.Database.Models.Entertaiment entertaiment = await _unitOfWork.Entertaiments.FindOneAsync(x=>x.Id==review.EntertaimentId);
                    entertaiment.Rating=(await _unitOfWork.Reviews.GetAll().Where(x=>x.EntertaimentId==review.EntertaimentId).ToListAsync()).Average(x=>x.Rating);
                    return result.Id;
                }
                throw new DatabaseException("Error saving review");
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error adding review: {errorMessage}", ex.Message);
                throw new DatabaseException("Error adding review", ex);
            }
        }
        public async Task<ReviewDto> GetReview(GetReviewRequest getReview)
        {
            try
            {
                Review review =await _unitOfWork.Reviews.FindOneAsync(x => x.Id == getReview.ReviewId);
                ReviewDto reviewDto = _mapper.Map<ReviewDto>(review);
                if(!review.IsAnonymous && review.UserId != null)
                {
                    GetUsernameAvatarResponse getUsernameAvatar = await GetUsernameAvatar(new GetUserName(){
                        UserId = (long)review.UserId
                    });
                    reviewDto.AuthorAvatar = getUsernameAvatar.Avatar;
                    reviewDto.AuthorName = getUsernameAvatar.Username;
                }
                reviewDto.Benefits = _mapper.ProjectTo<BenefitDto>(_unitOfWork.ReviewBenefits.GetAll().Where(x => x.ReviewId == getReview.ReviewId).Include(x => x.Benefit).Select(x => x.Benefit)).ToList();
                reviewDto.Feedbacks = _mapper.ProjectTo<FeedbackDto>(_unitOfWork.ReviewFeedbacks.GetAll().Where(x => x.ReviewId == getReview.ReviewId)).ToList();
                reviewDto.NegativeFeedbacksCount = reviewDto.Feedbacks.Count(x => x.IsPositive == false);
                reviewDto.PositiveFeedbacksCount = reviewDto.Feedbacks.Count(x => x.IsPositive == true);
                return reviewDto;
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error getting review: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting review", ex);
            }
        }
        public IQueryable<ReviewDto> GetReviews(GetReviewsRequest getReviews)
        {
            
            try
            {
                IQueryable<Review> currentReviews;
                if(getReviews.NewFirst)
                {
                    currentReviews = _unitOfWork.Reviews.GetAll().OrderByDescending(x=>x.Date).Skip((getReviews.Page - 1) * 10).Take(10);
                }
                else if(getReviews.RelevantFirst)
                {
                    currentReviews = _unitOfWork.Reviews.GetAll().Join(_unitOfWork.ReviewFeedbacks.GetAll().GroupBy(fb => fb.ReviewId).Select(g => new { ReviewId = g.Key, PositiveFeedbackCount = g.Count(fb => fb.IsPositive) }),
                        r => r.Id,
                        fb => fb.ReviewId,
                        (r, fb) => new { Review = r, PositiveFeedbackCount = fb.PositiveFeedbackCount })
                    .OrderByDescending(x => x.PositiveFeedbackCount)
                    .Skip((getReviews.Page - 1) * 10)
                    .Take(10)
                    .Select(x => x.Review);
                }
                else if(getReviews.PositiveFirst)
                {
                    currentReviews = _unitOfWork.Reviews.GetAll().OrderByDescending(x => x.Rating).Skip((getReviews.Page - 1) * 10).Take(10);
                }
                else if(getReviews.NegativeFirst)
                {
                    currentReviews = _unitOfWork.Reviews.GetAll().OrderBy(x => x.Rating).ThenBy(x => x.Date).Skip((getReviews.Page - 1) * 10).Take(10);
                }
                else
                {
                    currentReviews = _unitOfWork.Reviews.GetAll().Skip((getReviews.Page - 1) * 10).Take(10);
                }
                IQueryable<ReviewDto> reviewDtos = _mapper.ProjectTo<ReviewDto>(currentReviews);
                foreach(var review in currentReviews)
                {
                    var reviewDto = reviewDtos.FirstOrDefault(x => x.Id == review.Id)!;   
                    if(!review.IsAnonymous && review.UserId != null)
                    {
                        GetUsernameAvatarResponse getUsernameAvatar = GetUsernameAvatar(new GetUserName(){
                            UserId = (long)review.UserId
                        }).Result;
                        reviewDto.AuthorName = getUsernameAvatar.Username;
                        reviewDto.AuthorAvatar = getUsernameAvatar.Avatar;
                    }
                    reviewDto.Benefits = _mapper.ProjectTo<BenefitDto>(_unitOfWork.ReviewBenefits.GetAll().Where(x => x.ReviewId == review.Id).Include(x => x.Benefit).Select(x => x.Benefit)).ToList();
                    reviewDto.Feedbacks = _mapper.ProjectTo<FeedbackDto>(_unitOfWork.ReviewFeedbacks.GetAll().Where(x => x.ReviewId == review.Id)).ToList();
                    reviewDto.NegativeFeedbacksCount = reviewDto.Feedbacks.Count(x => x.IsPositive == false);
                    reviewDto.PositiveFeedbacksCount = reviewDto.Feedbacks.Count(x => x.IsPositive == true);
                }
                return reviewDtos;

            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting reviews: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting reviews", ex);
            }
        }
        public bool RemoveReview(RemoveReviewRequest removeReview)
        {
            
            try
            {
                Review review = _unitOfWork.Reviews.FindOneAsync(x => x.Id == removeReview.ReviewId).Result;
                review.Removed = true;
                _unitOfWork.Reviews.Update(review);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully removed review!");
                    return true;
                }
                throw new DatabaseException("Removing review went wrong!");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error removing review: {errorMessage}", ex.Message);
                throw new DatabaseException("Error removing review", ex);
            }
        }
        public bool UpdateReview(UpdateReviewRequest updateReview)
        {
            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                Review review = _unitOfWork.Reviews.FindOneAsync(x => x.Id == updateReview.ReviewId).Result;
                review.Text = updateReview.Text;
                if(updateReview.Benefits!=null)
                {
                    _unitOfWork.ReviewBenefits.DeleteMany(x=>x.ReviewId==updateReview.ReviewId);
                    foreach(var benefit in updateReview.Benefits)
                    {
                        _unitOfWork.ReviewBenefits.AddAsync(new ReviewBenefit(){
                            ReviewId = updateReview.ReviewId,
                            BenefitId = benefit
                        });
                    }
                }
                _unitOfWork.Reviews.Update(review);
                if(transaction.SaveAndCommit())
                {
                    _logger.LogDebug("Successefully updated review!");
                    return true;
                }
                throw new DatabaseException("Updating review went wrong!");
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error updating review: {errorMessage}", ex.Message);
                throw new DatabaseException("Error updating review", ex);
            }
           
        }
        public async Task<bool> RateReview(RateReviewRequest rateReview)
        {
            try
            {
                ReviewFeedback reviewFeedback = new ReviewFeedback(){
                    ReviewId = rateReview.ReviewId,
                    UserId = rateReview.UserId,
                    IsPositive = rateReview.Rating,
                };
                await _unitOfWork.ReviewFeedbacks.AddAsync(reviewFeedback);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully rated review!");
                    return true;
                }
                throw new DatabaseException("Rating review went wrong!");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error rating review: {errorMessage}", ex.Message);
                throw new DatabaseException("Error rating review", ex);
            }
        }

        private async Task<GetUsernameAvatarResponse> GetUsernameAvatar(GetUserName request)
        {
            try
            {
                Guid messageId = Guid.NewGuid();
                Message<string,string> message = new Message<string, string>()
                {
                    Key = messageId.ToString(),
                    Value = JsonConvert.SerializeObject(request),
                    Headers = new Headers()
                    {
                        new Header("method",Encoding.UTF8.GetBytes("getUsernameAvatar")),
                        new Header("sender",Encoding.UTF8.GetBytes("entertaimentService"))
                    }
                };
                if(await _kafkaRequestService.Produce(Environment.GetEnvironmentVariable("USER_REQUEST_TOPIC"),message,Environment.GetEnvironmentVariable("USER_RESPONSE_TOPIC")))
                {
                    _logger.LogDebug("Message sent :{messageId}",messageId.ToString());
                    while (!_kafkaRequestService.IsMessageRecieved(messageId.ToString()))
                    {
                        Thread.Sleep(200);
                    }
                    _logger.LogDebug("Message recieved :{messageId}",messageId.ToString());
                    return _kafkaRequestService.GetMessage<GetUsernameAvatarResponse>(messageId.ToString(),Environment.GetEnvironmentVariable("USER_RESPONSE_TOPIC"));
                }
                throw new ConsumerException("Message not recieved");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}