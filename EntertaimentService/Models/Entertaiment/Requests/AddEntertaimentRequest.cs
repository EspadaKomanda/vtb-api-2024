using System.ComponentModel.DataAnnotations;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Models.Entertaiment.Requests;

public class AddTourRequest
{
    [Required]
    [EntertaimentName]
    public string Name { get; set; } = null!;

    [Required]
    [EntertaimentDescription]
    public string Description { get; set; } = null!;

    [Required]
    [EntertaimentPrice]
    public double Price { get; set; }

    [Required]
    [EntertaimentAddress]
    public string Address { get; set; } = null!;

    [Coordinates]
    public string? Coordinates { get; set; }

    [Required]
    public bool IsActive { get; set; }

    public List<byte[]>? PhotoBytes { get; set; }
}