using PromoService.Models.Promocode.Requests;
using PromoService.Models.Promocode.Responses;

namespace PromoService.Services.Promocode;

public class PromocodeService : IPromocodeService
{
    public Task<CreatePromocodeResponse> CreatePromocode(CreatePromocodeRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<DeletePromocodeResponse> DeletePromocode(DeletePromocodeRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<SetPromocodeMetaResponse> SetPromocodeMeta(SetPromocodeMetaRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<SetPromocodeRestrictionsResponse> SetPromocodeRestrictions(SetPromocodeRestrictionsRequest request)
    {
        throw new NotImplementedException();
    }
}