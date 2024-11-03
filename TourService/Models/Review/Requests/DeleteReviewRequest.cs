using System.ComponentModel.DataAnnotations;

namespace TourService.TourReview.Requests;

public class DeleteReviewRequest
{
    [Required]
    public long ReviewId { get; set; }

    public string? RemovalReason { get; set; }
}