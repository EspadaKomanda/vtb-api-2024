using System.ComponentModel.DataAnnotations;

namespace PromoService.Database.Models;

public class UserPromo
{
    [Key]
    public long Id { get; set; } 

    public long UserId { get; set; }

    [Required]
    public Promo Promo { get; set; } = null!;
    public long PromoId { get; set; }


    [Required]
    public DateTime Date { get; set; }

    [Required]
    public double AmountDiscounted { get; set; }
}