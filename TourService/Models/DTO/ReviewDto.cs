using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Models.Benefits;

namespace TourService.Models.DTO
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorAvatar { get; set; }
        public string? Text { get; set; }
        public List<BenefitDto>? Benefits { get; set; }
        public int Rating { get; set; }
        public int PositiveFeedbacksCount { get; set; }
        public int NegativeFeedbacksCount { get; set; }
    }
}