using System.ComponentModel.DataAnnotations;

namespace PromoService.Models.Promocode.Requests;

public class DeletePromocodeRequest
{
    [Required]
    public long PromoId { get; set; }
}