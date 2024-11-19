using System.ComponentModel.DataAnnotations;

namespace EntertaimentService.TourReview.Requests;

public class RemoveReviewEntertainmentRequest
{
    [Required]
    public long ReviewId { get; set; }

    public string? RemovalReason { get; set; }
}