using System.ComponentModel.DataAnnotations;

namespace PromoService.Database.Models;

public class Promo
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string Code { get; set; } = null!;

    public string Description { get; set; } = null!;

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public bool IsStackable { get; set; }

    [Required]
    public float Discount { get; set; }

    public double? MaxAmountDiscounted { get; set; }

    public double? MaxPerUser { get; set; }

    public double? MaxPerEveryone { get; set; }

    public double? MinAccountAge { get; set; }

    /// <summary>
    /// Возраст аккаунта в часах
    /// </summary>
    public double? MaxAccountAge { get; set; }

    [Required]
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime? EndDate { get; set; }
}