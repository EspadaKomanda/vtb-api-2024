using System.ComponentModel.DataAnnotations;

namespace PromoService.Models.Promocode.Requests;

public class SetPromocodeMetaRequest
{
    [Required]
    public long PromoId { get; set; }
    [Required]
    public string Meta { get; set; } = null!;
}