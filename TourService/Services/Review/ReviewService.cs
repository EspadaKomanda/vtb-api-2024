using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Database.Models;
using TourService.Repository;

namespace TourService.Services.Review
{
    public class ReviewService : IReviewService
    {
        private readonly ILogger<IReviewService> _logger;
        private readonly IRepository<Review> _repository;
    }
}