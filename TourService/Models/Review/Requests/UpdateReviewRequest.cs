using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TourService.Attributes.Validation;

namespace TourService.Models.Review.Requests
{
    public class UpdateReviewRequest
    {
        [Required]
        public long ReviewId { get; set; }
        public long? UserId { get; set; }
        [Required]
        // TODO: [Comment] validation attribute
        public string Text { get; set; } = null!;
        public List<long>? Benefits { get; set; }
        [Required]
        [Rating]
        public int Rating { get; set; }
        public bool IsAnonymous { get; set; } = false;
    }
}