using System.ComponentModel.DataAnnotations;
using ApiGatewayService.Models.TourService.Models.Benefits;

namespace ApiGatewayService.Models.TourService.Models.Review.Requests;

public class AddReviewRequest
{
    [Required]
    public long TourId { get; set; }
    public long? UserId { get; set; }
    [Required]
    // TODO: [Comment] validation attribute
    public string Text { get; set; } = null!;

    public List<long>? Benefits { get; set; }

    [Required]
    public int Rating { get; set; }

    public bool IsAnonymous { get; set; } = false;
}