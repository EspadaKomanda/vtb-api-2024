using System.ComponentModel.DataAnnotations;

namespace PromoService.Models.PromoApplication.Requests;

public class ValidatePromocodeApplicationRequest
{
    [Required]
    public string PromoCode { get; set; } = null!;

    public List<long>? TourIds { get; set; }
    public List<long>? EntertainmentIds { get; set; }
}