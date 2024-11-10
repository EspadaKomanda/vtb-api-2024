using System.ComponentModel.DataAnnotations;

namespace PromoService.Database.Models;

public class PromoCategory
{
    [Key]
    public long Id { get; set; }

    [Required]
    public Promo Promo { get; set; } = null!;
    public long PromoId { get; set; }

    public long CategoryId { get; set; }
}