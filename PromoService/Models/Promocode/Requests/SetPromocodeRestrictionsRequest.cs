namespace PromoService.Models.Promocode.Requests;

public class SetPromocodeRestrictionsRequest
{
    public int PromocodeId { get; set; }

    public bool? IsStackable { get; set; }

    public double? MaxAmountDiscounted { get; set; }

    public int? MaxPerUser { get; set; }

    public long? MaxPerEveryone { get; set; }

    public double? MinAccountAge { get; set; }

    public double? MaxAccountAge { get; set; }

    public long? StartDate { get; set; }

    public long? EndDate { get; set; }

    public List<long>? Categories { get; set; }
}