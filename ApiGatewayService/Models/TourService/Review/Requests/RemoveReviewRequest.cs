using System.ComponentModel.DataAnnotations;

namespace ApiGatewayService.Models.TourService.TourReview.Requests;

public class RemoveReviewRequest
{
    [Required]
    public long ReviewId { get; set; }

    public string? RemovalReason { get; set; }
}