using System.ComponentModel.DataAnnotations;

namespace EntertaimentService.Models.Entertaiment.Requests;

public class AddEntertaimentRequest
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public double Price { get; set; }

    [Required]
    public string Address { get; set; } = null!;

    public string? Coordinates { get; set; }
    public string? Comment { get; set; }
    [Required]
    public bool IsActive { get; set; }

    public List<byte[]>? PhotoBytes { get; set; }
}