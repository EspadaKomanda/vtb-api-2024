using System.ComponentModel.DataAnnotations;
using TourService.Attributes.Validation;

namespace TourService.Models.Tour.Requests;

public class AddTourRequest
{
    [Required]
    [TourName]
    public string Name { get; set; } = null!;

    [Required]
    [TourDescription]
    public string Description { get; set; } = null!;

    [Required]
    [TourPrice]
    public double Price { get; set; }

    [Required]
    [TourAddress]
    public string Address { get; set; } = null!;

    [Coordinates]
    public string? Coordinates { get; set; }

    public double? SettlementDistance { get; set; }

    [Required]
    public bool IsActive { get; set; }
}