using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.Benefits;

namespace EntertaimentService.Models.DTO
{
    public class ReviewEntertaimentDto
    {
        public long Id { get; set; }
        public long EntertaimentId { get; set; }
        public DateTime Date { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorAvatar { get; set; }
        public string? Text { get; set; }
        public List<BenefitEntertainmentDto>? Benefits { get; set; }
        public List<FeedbackEntertaimentDto>? Feedbacks { get; set; }
        public int Rating { get; set; }
        public int PositiveFeedbacksCount { get; set; }
        public int NegativeFeedbacksCount { get; set; }
    }
}