using System.ComponentModel.DataAnnotations;

namespace EntertaimentService.Models.Review.Requests;

public class GetReviewsRequest
{
    [Required]
    public long EntertaimentId { get; set; }
    public int Page { get; set; } = 0;
    public bool PositiveFirst { get; set; } = false;
    public bool NegativeFirst { get; set; } = false;
    public bool RelevantFirst { get; set; } = false;   
    public bool NewFirst { get; set; } = false;
}