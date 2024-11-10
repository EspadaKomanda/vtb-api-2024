using System.ComponentModel.DataAnnotations;
namespace PromoService.Models.Promocode.Requests;

public class CreatePromocodeRequest
{
  // TODO: implement auto-generation
  public string? Code { get; set; }

  [Required]
  public string Description { get; set; } = null!;

  [Required]
  public bool IsActive { get; set; }

  public bool IsStackable { get; set; } = true;

  [Required]
  public float Discount { get; set; }

  public double? MaxAmountDiscounted { get; set; }

  public int MaxPerUser { get; set; } = 1;

  public long? MaxPerEveryone { get; set; }

  public long? MinAccountAge { get; set; }

  public long? MaxAccountAge { get; set; }

  public DateTime StartDate { get; set; } = DateTime.UtcNow;

  [Required]
  public DateTime EndDate { get; set; }
}
