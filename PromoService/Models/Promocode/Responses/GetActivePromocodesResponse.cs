using PromoService.Database.Models;

namespace PromoService.Models.Promocode.Responses;

public class GetActivePromocodesResponse
{
    public ICollection<Promo> Promocodes { get; set; } = null!;
}