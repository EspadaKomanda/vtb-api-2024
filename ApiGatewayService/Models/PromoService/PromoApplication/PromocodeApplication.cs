namespace ApiGatewayService.Models.PromoService;

public class PromocodeApplication
{
    public long PromocodeId { get; set; }
    public List<long?> TourIds { get; set; } = null!;
    public List<long?> EntertainmentIds { get; set; } = null!;
}