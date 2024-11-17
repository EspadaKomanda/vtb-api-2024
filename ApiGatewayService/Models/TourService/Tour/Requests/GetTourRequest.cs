using System.ComponentModel.DataAnnotations;

namespace TourService.Models.Tour.Requests;

public class GetTourRequest
{
    [Required]
    public long Id { get; set; }
}