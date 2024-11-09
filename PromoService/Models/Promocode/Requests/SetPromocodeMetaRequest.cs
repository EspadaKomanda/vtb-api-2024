using System.ComponentModel.DataAnnotations;

namespace PromoService.Models.Promocode.Requests;

public class SetPromocodeMetaRequest
{
    [Required]
    public long PromoId { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsStackable { get; set; }

    public float? Discount { get; set; }

    public double? MaxAmountDiscounted { get; set; }

    public double? MaxPerUser { get; set; }

    public double? MaxPerEveryone { get; set; }

    public double? MinAccountAge { get; set; }

    public double? MaxAccountAge { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? Deleted { get; set; }
}