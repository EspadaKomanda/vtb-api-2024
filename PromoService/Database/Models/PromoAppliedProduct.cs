using System.ComponentModel.DataAnnotations;

namespace PromoService.Database.Models;

public class PromoAppliedProduct
{
    [Key]
    public long Id { get; set; }

    [Required]
    public UserPromo UserPromo { get; set; } = null!;
    public long UserPromoId { get; set; }

    public long? TourId { get; set; }
    public long? EntertainmentId { get; set; }
}