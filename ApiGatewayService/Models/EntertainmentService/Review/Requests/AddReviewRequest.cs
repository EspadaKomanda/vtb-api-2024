using System.ComponentModel.DataAnnotations;

namespace EntertaimentService.Models.Review.Requests;

public class AddReviewEntertaimentRequest
{
    [Required]
    public long EntertaimentId { get; set; }
    public long? UserId { get; set; }
    [Required]
    // TODO: [Comment] validation attribute
    public string Text { get; set; } = null!;

    public List<long>? Benefits { get; set; }

    [Required]
    public int Rating { get; set; }

    public bool IsAnonymous { get; set; } = false;
}