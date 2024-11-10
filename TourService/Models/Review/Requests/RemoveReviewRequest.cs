using System.ComponentModel.DataAnnotations;

namespace TourService.TourReview.Requests;

public class RemoveReviewRequest
{
    [Required]
    public long ReviewId { get; set; }

    public string? RemovalReason { get; set; }
}