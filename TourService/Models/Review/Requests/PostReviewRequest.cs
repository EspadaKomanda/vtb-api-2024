using System.ComponentModel.DataAnnotations;
using TourService.Attributes.Validation;

namespace TourService.Models.Review.Requests;

public class PostReviewRequest
{
    [Required]
    public long TourId { get; set; }

    [Required]
    // TODO: [Comment] validation attribute
    public string Text { get; set; } = null!;

    public List<string>? Benefits { get; set; }

    [Required]
    [Rating]
    public int Rating { get; set; }

    public bool IsAnonymous { get; set; } = false;
}