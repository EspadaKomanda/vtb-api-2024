using Entertaiment.Attributes.Validation;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Models.Tour.Requests;

// TODO: finish this
public class GetEntertaimentsRequest
{
    public int Page { get; set; } = 0;
    public List<long>? Categories { get; set; }
    [Rating]
    public int MinimalRating { get; set; } = 0;
    [Rating]
    public int MaximalRating { get; set; } = 5;
    public double MinimalPrice { get; set; } = 0;
    public double MaximalPrice { get; set; } = double.MaxValue;
}