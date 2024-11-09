using System.ComponentModel.DataAnnotations;
using TourService.Attributes.Validation;
using TourService.Models.Benefits;

namespace TourService.Models.Review.Requests;

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
    [Rating]
    public int Rating { get; set; }

    public bool IsAnonymous { get; set; } = false;
}