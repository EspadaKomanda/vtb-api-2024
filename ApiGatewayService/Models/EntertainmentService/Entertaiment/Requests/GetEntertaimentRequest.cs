using System.ComponentModel.DataAnnotations;

namespace EntertaimentService.Models.Tour.Requests;

public class GetEntertaimentRequest
{
    [Required]
    public long Id { get; set; }
}